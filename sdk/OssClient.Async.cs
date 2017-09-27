#if NETSTANDARD2_0 || NET40

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Aliyun.OSS
{
    /// <summary>
    /// 扩展支持TPL方式
    /// </summary>
    public static class OssClientAsync
    {
        #region Object Operations

        public static Task<ObjectListing> ListObjectsAsync(this IOss client,
            string bucketName,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<ObjectListing>((cts) =>
            {
                client.BeginListObjects(bucketName, (ar) =>
                {
                    var result = client.EndListObjects(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        public static Task<ObjectListing> ListObjectsAsync(this IOss client,
            ListObjectsRequest listObjectsRequest,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<ObjectListing>((cts) =>
            {
                client.BeginListObjects(listObjectsRequest, (ar) =>
                {
                    var result = client.EndListObjects(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        public static Task<PutObjectResult> PutObjectAsync(this IOss client,
            string bucketName, string key, Stream stream,
            CancellationToken token = default(CancellationToken))
        {
            return client.PutObjectAsync(bucketName, key, stream, null, token);
        }

        public static Task<PutObjectResult> PutObjectAsync(this IOss client,
            string bucketName, string key, Stream content, ObjectMetadata metadata,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<PutObjectResult>((cts) =>
            {
                client.BeginPutObject(bucketName, key, content, metadata, (ar) =>
                {
                    var result = client.EndPutObject(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        public static Task<PutObjectResult> PutObjectAsync(this IOss client,
            PutObjectRequest putObjectRequest,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<PutObjectResult>((cts) =>
            {
                client.BeginPutObject(putObjectRequest, (ar) =>
                {
                    var result = client.EndPutObject(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        public static Task<AppendObjectResult> AppendObjectAsync(this IOss client,
            AppendObjectRequest request,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<AppendObjectResult>((cts) =>
            {
                client.BeginAppendObject(request, (ar) =>
                {
                    var result = client.EndAppendObject(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        public static Task<OssObject> GetObjectAsync(this IOss client,
            string bucketName,
            string key,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<OssObject>((cts) =>
            {
                client.BeginGetObject(bucketName, key, (ar) =>
                {
                    var result = client.EndGetObject(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        public static Task<OssObject> GetObjectAsync(this IOss client,
            GetObjectRequest getObjectRequest,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<OssObject>((cts) =>
            {
                client.BeginGetObject(getObjectRequest, (ar) =>
                {
                    var result = client.EndGetObject(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        public static Task<CopyObjectResult> CopyObjectAsync(this IOss client,
            CopyObjectRequest copyObjectRequst,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<CopyObjectResult>((cts) =>
            {
                client.BeginCopyObject(copyObjectRequst, (ar) =>
                {
                    var result = client.EndCopyResult(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        #endregion Object Operations

        #region Multipart Operations

        public static Task<UploadPartResult> UploadPartAsync(this IOss client,
            UploadPartRequest uploadPartRequest,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<UploadPartResult>((cts) =>
            {
                client.BeginUploadPart(uploadPartRequest, (ar) =>
                {
                    var result = client.EndUploadPart(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        public static Task<UploadPartCopyResult> UploadPartCopyAsync(this IOss client,
            UploadPartCopyRequest uploadPartCopyRequest,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync<UploadPartCopyResult>((cts) =>
            {
                client.BeginUploadPartCopy(uploadPartCopyRequest, (ar) =>
                {
                    var result = client.EndUploadPartCopy(ar);
                    if (token.IsCancellationRequested)
                    {
                        cts.TrySetCanceled();
                    }
                    else
                    {
                        cts.TrySetResult(result);
                    }
                }, null);
            }, token);
        }

        #endregion Multipart Operations

        private static Task<T> ExecuteAsync<T>(
            Action<TaskCompletionSource<T>> cts,
            CancellationToken token = default(CancellationToken))
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            try
            {
                cts(taskCompletionSource);

                token.Register(() =>
                {
                    taskCompletionSource.TrySetCanceled();
                });
            }
            catch (Exception ex)
            {
                taskCompletionSource.TrySetException(ex);
            }

            return taskCompletionSource.Task;
        }
    }
}

#endif