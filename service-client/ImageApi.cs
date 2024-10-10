using service_client;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace service_client
{
    public class ImageApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:8080/api/images"; // 기본 API URL 설정

        public ImageApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // 이미지 저장
        public async Task<bool> PostImageAsync(string filePath)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        // 이미지 파일을 읽어 파일 컨텐츠로 설정
                        var imageContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                        // 랜덤 UUID 생성
                        string randomFileName = $"test_{Guid.NewGuid()}.jpg";

                        // Multipart 폼에 이미지 추가 ("image"는 Spring 서버에서 기대하는 이름)
                        formData.Add(imageContent, "image", randomFileName);

                        // HttpREquestMessage 생성
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/api/images")
                        {
                            Content = formData
                        };

                        // SendAsync 메서드 요청 전송
                        var response = await client.SendAsync(request);

                        // 응답 상태 코드에 따라 처리
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("서버 응답: " + responseContent);
                            return true; // 성공
                        }
                        else if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            Console.WriteLine("잘못된 요청: " + response.StatusCode);
                            return false; // 잘못된 요청
                        }
                        else
                        {
                            Console.WriteLine("요청 실패: " + response.StatusCode);
                            return false; // 다른 오류
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("오류 발생: " + ex.Message);
                return false; // 예외 발생 시 false 반환
            }
        }
    }


}
