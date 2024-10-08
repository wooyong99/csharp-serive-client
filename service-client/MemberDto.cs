using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace service_client
{
    public class Member
    {
        public int Id { get; set; }
        public string LoginId { get; set; }
        public string Name { get; set; }

        // ToString() 메서드 오버라이드
        public override string ToString()
        {
            return $"ID: {Id}, LoginId: {LoginId}, Name: {Name}";
        }
    }

    public class MemberListResponse
    {
        public int Id { get; set; }
        public string LoginId { get; set; }

        // ToString() 메서드 오버라이드
        public override string ToString()
        {
            return $"ID: {Id}, LoginId: {LoginId}";
        }
    }

    // 회원가입 요청 DTO
    public class SignupRequest
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        // ToString() 메서드 오버라이드
        public override string ToString()
        {
            return $"LoginId: {LoginId}, Password: {Password}, Name: {Name}";
        }
    }

    // 로그인 요청 DTO
    public class LoginRequest
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
    }

    // 회원 이름 수정 요청 DTO
    public class UpdateNameRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
