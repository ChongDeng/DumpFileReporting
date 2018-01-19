using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Net;

namespace DumpFileReporting
{
    class UploadObject
    {
        //static string bucketName = "mobile-analytics-dc-10124024";
        static string bucketName = "dc0119";
        static string keyName = "qq.jpg";
        //static string filePath = "C:\\Users\\fqyya\\Desktop\\qq.jpg";
        static string filePath = "qq.jpg";

        static IAmazonS3 client;

        public static void Main(string[] args)
        {
            try
            {
                using (client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                {
                    if (!(client.DoesS3BucketExist(bucketName)))
                    {                       
                        client.PutBucket(bucketName);
                        Console.WriteLine("Created a bucket: " + bucketName);
                    }

                    Console.WriteLine("Uploading an object");
                    WritingAnObject();
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("ex:" + ex);
                Console.ReadKey();
            }


        }

        static void WritingAnObject()
        {
            try
            {
                PutObjectRequest putRequest1 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    FilePath = filePath
                };

                PutObjectResponse response = client.PutObject(putRequest1);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    Console.Out.WriteLine("upload sucessfully");
                }
                else
                {
                    Console.Out.WriteLine("upload failed");
                }


            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine(
                        "For service sign up go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine(
                        "Error occurred. Message:'{0}' when writing an object"
                        , amazonS3Exception.Message);
                }
            }
        }
    }
}
