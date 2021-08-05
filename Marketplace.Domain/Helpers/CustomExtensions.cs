﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Marketplace.Domain.Helpers
{
    public static class CustomExtensions
    {
        public static bool IsEmpty<T>(this List<T> lst) => (lst == null || !lst.Any());
        public static bool IsEmpty(this string vl) => string.IsNullOrWhiteSpace(vl);
        public static bool IsNotEmpty(this string vl) => !string.IsNullOrWhiteSpace(vl);
        public static bool IsEmpty(this IFormFile vl) => vl == null || vl.Length <= 0;
        public static bool IsNumber(this string vl) => int.TryParse(vl, out int ss);
        public static int ToInt(this string vl) => int.Parse(vl);
        public static DateTime toDate(this string vl) => DateTime.Parse(vl);

        public static DateTime DateNow
        {
            get
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    return DateTime.Now;
                else
                {
                    TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
                }
            }
        }
        public static string IsCompare(this string vl) => RemoveAccents(vl).ToLower().Trim();
        public static string Clear(this string vl) => RemoveAccents(vl).Trim();

        public static string createHash(this string str)
        {
            if (str.IsEmpty())
                return null;

            byte[] hash = (new SHA256Managed()).ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder hashString = new StringBuilder();
            foreach (byte x in hash)
            {
                hashString.Append(String.Format("{0:x2}", x));
            }
            return hashString.ToString();
        }

        public static string RemoveAccents(this string text)
        {
            if (text == null) text = "";

            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string Serialize(this object vl) => Newtonsoft.Json.JsonConvert.SerializeObject(vl);
        public static T Deserialize<T>(this string vl) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(vl);
        public static string getGuid => Guid.NewGuid().ToString();

        public static string getExtension(this string vl) => vl.Split(".")[vl.Split(".").Length - 1];
        public static bool IsEmail(this string vl) => (new EmailAddressAttribute()).IsValid(vl);
        public static bool IsCnpj(this string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
        public static bool IsCpf(this string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static string GenerateToken(Models.dto.auth.AuthDto auth, string _secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = CustomExtensions.DateNow.AddDays(1),
                Subject = new ClaimsIdentity()
            };
            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.UserData, auth.Serialize()));

            // perfis
            auth.rules.ForEach(role =>
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
            });

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}