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
    public class MemberApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:8080/api/members"; // 기본 API URL 설정

        public MemberApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // 단일 회원 조회
        public async Task<Member> GetMemberByIdAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/{id}");
            var response = await _httpClient.SendAsync(request);
            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // JSON을 Member 객체로 변환
            return JsonSerializer.Deserialize<Member>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // CamelCase로 변환 옵션
            });
        }

        // 회원 가입
        public async Task<bool> SignupAsync(SignupRequest signupRequest)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // CamelCase로 변환 옵션
            };
            
            // SignupRequest를 JSON 문자열로 직렬화
            var jsonContent = JsonSerializer.Serialize(signupRequest, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // HttpRequestMessage 생성 (POST 요청)
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/signup")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            // 응답 상태 코드에 따라 반환값 결정
            if (response.StatusCode == HttpStatusCode.Created) // 200
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest) // 400
            {
                return false;
            }

            // 다른 상태 코드에 대한 처리 (필요하다면)
            return false; // 기본값으로 false를 반환 (예: 500 에러 등)
        }

        // 회원 정보 수정 (이름)
        public async Task<bool> UpdateMemberNameAsync(UpdateNameRequest updateNameRequest)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // CamelCase로 변환 옵션
            };


            // UpdateMemberNameRequest를 JSON 문자열로직렬화
            var jsonContent = JsonSerializer.Serialize(updateNameRequest, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // HttpRequestMessage 생성 (PUT 요청)
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/{updateNameRequest.Id}")
            {
                Content = content
            };

            // 요청 전송
            var response = await _httpClient.SendAsync(request);

            // 응답 상태 코드에 따라 반환값 결정
            if (response.StatusCode == HttpStatusCode.NoContent) // 204 OK
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest) // 400 Bad Request
            {
                return false;
            }

            // 다른 상태 코드에 대한 처리 (필요하다면)
            return false; // 기본값으로 false를 반환 (예: 500 에러 등)
        }

        // 회원 삭제
        public async Task<bool> DeleteMemberAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{id}");
            Console.WriteLine(request.RequestUri);
            var response = await _httpClient.SendAsync(request);
            
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 전체 회원 조회
        public async Task<MemberListResponse[]> GetMemberAllAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/all");
            var response = await _httpClient.SendAsync(request);

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // JSON을 MemberListResponse 객체 배열로 변환
            return JsonSerializer.Deserialize<MemberListResponse[]>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // CamelCase로 속성 이름 변환 옵션
            });
        }
    }
}
