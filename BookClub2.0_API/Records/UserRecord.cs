﻿using BookClub2._0.Models;
using BookClub2._0.Repositories;
using BookClub2._0.Interfaces;
namespace BookClub2._0_API.Records
{
    public record UserRecord (int Id, string UserName, string Email, string Password, string Role);

    public static class Recordhelper
    {
        public static User ConvertUserRecord(UserRecord record)
        {
            if (record.Id == null) { throw new ArgumentNullException("Exception" + record.Id); }
            if (record.UserName == null) { throw new ArgumentNullException("Exception" + record.UserName); }
            if (record.Email == null) { throw new ArgumentNullException("Exception" + record.Email); }
            if (record.Password == null) { throw new ArgumentNullException("Exception" + record.Password); }
            if (record.Role == null) { throw new ArgumentNullException("Exception" + record.Role); }

            return new User() {Id = (int)record.Id, Email = record.Email, PasswordHash = record.Password, Role = record.Role, UserName = record.UserName };

        }
    }

}
