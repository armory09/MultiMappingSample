using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using Dapper;
using MultiMappingInDapper.Models;
using MultiMappingInDapper;

namespace MultiMappingInDapper
{
    static class Programaasdasd
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Multi mapping dapper");

            var conn = GetOpenConnection();

            var query = "SELECT * from Student stu where id = 1 SELECT * from Enrollment enr where enr.StudentId in (SELECT id from Student stu where id = 1)";

            //var mapped = conn.QueryMultiple(query)
            //    .Map<Contact, Phone, int>(
            //        contact => contact.ContactID,
            //        phone => phone.ContactID,
            //        ((contact, phones) => { contact.Phones = phones; })
            //    );

            var mapped2 = conn.QueryMultiple(query)
                .Map<Student, Enrollment, int>(
                    student => student.Id,
                    enrollment => enrollment.StudentId,
                    ((student, enrollments) => { student.Enrollments = enrollments; })
                );

            foreach (var student in mapped2)
            {
                Console.WriteLine("Name: " + student.FirstMidName + " Lastname: " + student.Lastname + " Enrollment Date: " + student.EnrollmentDate);

                foreach (var enrollment in student.Enrollments)
                {
                    Console.WriteLine("Grade: " + enrollment.Grade);
                }
            }
            Console.ReadLine();
        }

        private static IDbConnection GetOpenConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DbString"].ConnectionString;

            var connection = new SqlConnection(connectionString);

            return connection;
        }
        public static IEnumerable<TFirst> Map<TFirst, TSecond, TKey>
            (
            this SqlMapper.GridReader reader,
            Func<TFirst, TKey> firstKey,
            Func<TSecond, TKey> secondKey,
            Action<TFirst, IEnumerable<TSecond>> addChildren
            )
        {
            var first = reader.Read<TFirst>().ToList();
            var childMap = reader
                .Read<TSecond>()
                .GroupBy(secondKey)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var item in first)
            {
                IEnumerable<TSecond> children;
                if (childMap.TryGetValue(firstKey(item), out children))
                {
                    addChildren(item, children);
                }
            }

            return first;
        }
    }
}
