
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace LightweightAPI
{
    public class SimpleHttpServer : IDisposable
    {
        private HttpListener _listener;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _listenerTask;
        private readonly Action<string, string> _logger;
        private ISimpleItemService _itemService;

        public SimpleHttpServer(Action<string, string> logger = null)
        {
            _logger = logger;
        }

        public bool IsRunning => _listener?.IsListening == true;

        public void SetItemService(ISimpleItemService itemService)
        {
            _itemService = itemService;
        }

        public Task StartAsync(string url = "http://localhost:8080/")
        {
            if (_listener != null)
            {
                _logger?.Invoke("WARNING", "HTTP server is already running");
                return Task.CompletedTask;
            }

            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add(url.EndsWith("/") ? url : url + "/");
                _listener.Start();

                _cancellationTokenSource = new CancellationTokenSource();
                _listenerTask = ListenForRequestsAsync(_cancellationTokenSource.Token);

                _logger?.Invoke("INFO", $"HTTP server started at {url}");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger?.Invoke("ERROR", $"Failed to start HTTP server: {ex.Message}");
                throw;
            }
        }

        public async Task StopAsync()
        {
            if (_listener == null)
            {
                _logger?.Invoke("WARNING", "HTTP server is not running");
                return;
            }

            try
            {
                _logger?.Invoke("INFO", "Stopping HTTP server...");

                _cancellationTokenSource?.Cancel();

                if (_listener.IsListening)
                    _listener.Stop();

                if (_listenerTask != null)
                {
                    try { await _listenerTask.ConfigureAwait(false); }
                    catch (OperationCanceledException) { }
                    catch (HttpListenerException) { }
                }

                _listener.Close();
                _listener = null;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _listenerTask = null;

                _logger?.Invoke("INFO", "HTTP server stopped");
            }
            catch (Exception ex)
            {
                _logger?.Invoke("ERROR", $"Error stopping HTTP server: {ex.Message}");
                throw;
            }
        }

        private async Task ListenForRequestsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _listener?.IsListening == true)
            {
                try
                {
                    var context = await _listener.GetContextAsync().ConfigureAwait(false);
                    _ = Task.Run(() => HandleRequestAsync(context), cancellationToken);
                }
                catch (ObjectDisposedException) { break; }
                catch (HttpListenerException ex) when (ex.ErrorCode == 995) { break; }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger?.Invoke("ERROR", $"Error getting HTTP context: {ex.Message}");
                }
            }
        }

        private async Task HandleRequestAsync(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                var response = context.Response;

                // CORS headers
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                if (request.HttpMethod == "OPTIONS")
                {
                    response.StatusCode = 200;
                    response.Close();
                    return;
                }

                var path = request.Url?.AbsolutePath?.TrimEnd('/') ?? "";
                var method = request.HttpMethod;
                string responseJson;

                switch ($"{method} {path}")
                {
                    case "GET /":
                    case "GET /api/status":
                        responseJson = GetStatus();
                        break;
                    case "GET /api/items":
                        responseJson = GetAllItems();
                        break;
                    case "POST /api/cycle":
                        responseJson = CycleItem();
                        break;
                    case "POST /api/cycle-tier":
                        responseJson = CycleTier();
                        break;
                    case "POST /api/toggle":
                        responseJson = Toggle();
                        break;
                    case "POST /api/enable":
                        responseJson = Enable();
                        break;
                    case "POST /api/disable":
                        responseJson = Disable();
                        break;
                    case "POST /api/set-item":
                        responseJson = await SetItemByName(request);
                        break;
                    default:
                        response.StatusCode = 404;
                        responseJson = $"{{\"error\":\"Not Found\",\"path\":\"{path}\",\"method\":\"{method}\"}}";
                        break;
                }

                var buffer = Encoding.UTF8.GetBytes(responseJson);
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                response.StatusCode = response.StatusCode == 0 ? 200 : response.StatusCode;

                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                response.Close();
            }
            catch (Exception ex)
            {
                _logger?.Invoke("ERROR", $"Error handling request: {ex.Message}");
                try
                {
                    context.Response.StatusCode = 500;
                    context.Response.Close();
                }
                catch { }
            }
        }

        private string GetStatus()
        {
            var status = _itemService?.GetCurrentStatus() ?? "No item service";
            var enabled = _itemService?.IsEnabled ?? false;
            return $"{{\"status\":\"{status}\",\"enabled\":{enabled.ToString().ToLower()},\"timestamp\":\"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\"}}";
        }

        private string GetAllItems()
        {
            return _itemService?.GetAllItems() ?? "{\"items\":[],\"count\":0,\"message\":\"No item service\"}";
        }

        private string CycleItem()
        {
            _itemService?.CycleItem();
            var status = _itemService?.GetCurrentStatus() ?? "No item service";
            return $"{{\"message\":\"Item cycled\",\"status\":\"{status}\",\"timestamp\":\"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\"}}";
        }

        private string CycleTier()
        {
            _itemService?.CycleTier();
            var status = _itemService?.GetCurrentStatus() ?? "No item service";
            return $"{{\"message\":\"Tier cycled\",\"status\":\"{status}\",\"timestamp\":\"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\"}}";
        }

        private string Toggle()
        {
            _itemService?.ToggleEnabled();
            var status = _itemService?.GetCurrentStatus() ?? "No item service";
            var enabled = _itemService?.IsEnabled ?? false;
            return $"{{\"message\":\"Toggled\",\"status\":\"{status}\",\"enabled\":{enabled.ToString().ToLower()},\"timestamp\":\"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\"}}";
        }

        private string Enable()
        {
            _itemService?.Enable();
            var status = _itemService?.GetCurrentStatus() ?? "No item service";
            return $"{{\"message\":\"Enabled\",\"status\":\"{status}\",\"timestamp\":\"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\"}}";
        }

        private string Disable()
        {
            _itemService?.Disable();
            var status = _itemService?.GetCurrentStatus() ?? "No item service";
            return $"{{\"message\":\"Disabled\",\"status\":\"{status}\",\"timestamp\":\"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\"}}";
        }

        private async Task<string> SetItemByName(HttpListenerRequest request)
        {
            try
            {
                // Read the request body
                string requestBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                // Simple JSON parsing to extract itemName
                string itemName = null;
                if (!string.IsNullOrEmpty(requestBody))
                {
                    // Look for "itemName":"value" pattern
                    var startIndex = requestBody.IndexOf("\"itemName\"");
                    if (startIndex >= 0)
                    {
                        var colonIndex = requestBody.IndexOf(':', startIndex);
                        if (colonIndex >= 0)
                        {
                            var valueStart = requestBody.IndexOf('"', colonIndex) + 1;
                            var valueEnd = requestBody.IndexOf('"', valueStart);
                            if (valueStart > 0 && valueEnd > valueStart)
                            {
                                itemName = requestBody.Substring(valueStart, valueEnd - valueStart);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(itemName))
                {
                    return "{\"error\":\"Missing itemName in request body\",\"message\":\"Expected JSON: {\\\"itemName\\\":\\\"ItemName\\\"}\"}";
                }

                _itemService?.SetItemByName(itemName);
                var status = _itemService?.GetCurrentStatus() ?? "No item service";
                return $"{{\"message\":\"Item set to {itemName}\",\"status\":\"{status}\",\"timestamp\":\"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\"}}";
            }
            catch (Exception ex)
            {
                return $"{{\"error\":\"Failed to set item\",\"message\":\"{ex.Message}\",\"timestamp\":\"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\"}}";
            }
        }

        public void Dispose()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                if (_listener?.IsListening == true) _listener.Stop();
                _listener?.Close();
            }
            catch { }
            finally
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _listener = null;
                _listenerTask = null;
            }
        }
    }
}
