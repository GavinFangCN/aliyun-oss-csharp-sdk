using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS.Common;

namespace Aliyun.OSS.Samples
{
    public static class AsyncSample
    {
        static string accessKeyId = Config.AccessKeyId;
        static string accessKeySecret = Config.AccessKeySecret;
        static string endpoint = Config.Endpoint;
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        static string fileToUpload = Config.FileToUpload;
        static string dirToDownload = Config.DirToDownload;

        public static async Task PutObjectFromFile(string bucketName)
        {
            const string key = "PutObjectFromFile";
            try
            {
                PutObjectResult result;
                using (var stream = File.OpenRead(fileToUpload))
                {
                    result = await client.PutObjectAsync(bucketName, key, stream);
                }

                Console.WriteLine("Put object:{0} succeeded,HttpStatusCode:{1}", key, result.HttpStatusCode);

            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

        public static async Task GetObject(string bucketName)
        {
            try
            {
                string key = "GetObjectSample";
                using (var stream = File.OpenRead(fileToUpload))
                {
                    await client.PutObjectAsync(bucketName, key, stream);
                }

                var result = await client.GetObjectAsync(bucketName, key);

                using (var requestStream = result.Content)
                {
                    using (var fs = File.Open(dirToDownload + "/sample.data", FileMode.OpenOrCreate))
                    {
                        int length = 4 * 1024;
                        var buf = new byte[length];
                        do
                        {
                            length = requestStream.Read(buf, 0, length);
                            fs.Write(buf, 0, length);
                        } while (length != 0);
                    }
                }

                Console.WriteLine("Get object succeeded");
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

        public static async Task SimpleListObjects(string bucketName)
        {
            try
            {
                var result = await client.ListObjectsAsync(bucketName);

                Console.WriteLine("List objects of bucket:{0} succeeded ", bucketName);
                foreach (var summary in result.ObjectSummaries)
                {
                    Console.WriteLine(summary.Key);
                }

                Console.WriteLine("List objects of bucket:{0} succeeded, is list all objects ? {1}", bucketName, !result.IsTruncated);
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

        public static async Task ListObjectsWithRequest(string bucketName)
        {
            try
            {
                var keys = new List<string>();
                ObjectListing result = null;
                string nextMarker = string.Empty;
                do
                {
                    var listObjectsRequest = new ListObjectsRequest(bucketName)
                    {
                        Marker = nextMarker,
                        MaxKeys = 100
                    };
                    result = await client.ListObjectsAsync(listObjectsRequest);

                    foreach (var summary in result.ObjectSummaries)
                    {
                        Console.WriteLine(summary.Key);
                        keys.Add(summary.Key);
                    }

                    nextMarker = result.NextMarker;
                } while (result.IsTruncated);

                Console.WriteLine("List objects of bucket:{0} succeeded ", bucketName);
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

        public static async Task AppendObject(string bucketName)
        {
            const string key = "AppendObject";
            long position = 0;
            try
            {
                var metadata = client.GetObjectMetadata(bucketName, key);
                position = metadata.ContentLength;
            }
            catch (Exception) { }

            try
            {
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var request = new AppendObjectRequest(bucketName, key)
                    {
                        ObjectMetadata = new ObjectMetadata(),
                        Content = fs,
                        Position = position
                    };

                    var result = await client.AppendObjectAsync(request);
                    position = result.NextAppendPosition;

                    Console.WriteLine("Append object succeeded, next append position:{0}", position);
                }

                // append object by using NextAppendPosition
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var request = new AppendObjectRequest(bucketName, key)
                    {
                        ObjectMetadata = new ObjectMetadata(),
                        Content = fs,
                        Position = position
                    };

                    var result = client.AppendObject(request);
                    position = result.NextAppendPosition;

                    Console.WriteLine("Append object succeeded too, next append position:{0}", position);
                }
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

    }


}
