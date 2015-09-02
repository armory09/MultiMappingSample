using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MultiMappingInDapper.Models;

namespace MultiMappingInDapper
{
    static class Program
    {


        static void Main()
        {

            Console.WriteLine("Multi mapping dapper");
            var id = Convert.ToInt32(Console.ReadLine());


            var conn = GetOpenConnection();

            const string query = "SELECT * from Student stu where StudentId = @id; " +
                                 "SELECT * from Enrollment enr where enr.StudentId in (SELECT StudentId from Student stu where StudentId = @id); " +
                                 "SELECT * FROM Course cou WHERE cou.CourseId in (SELECT CourseId from Enrollment where StudentId = @id);";

            var enrollLst = new List<Enrollment>();
            var ctr = 0;
            var mapped2 = conn.QueryMultiple(query, new { id })
                .Map<Student, Enrollment, Course, int>(
                    student => student.StudentId,
                    enrollment => ctr = enrollment.StudentId,
                    course => course.CourseId = ctr,
                    ((student, enrollments) =>
                    {
                        enrollLst = enrollments.ToList();
                    }),
                    ((student, courses) =>
                    {
                        var ctrId = 0;

                        courses.ToList().ForEach(cour =>
                        {
                            enrollLst[ctrId].Course = cour;
                            ctrId++;

                        });

                        student.Enrollments = enrollLst;
                    }));



            foreach (var student in mapped2)
            {
                Console.WriteLine("Name: " + student.FirstMidName + " Lastname: " + student.Lastname + " Enrollment Date: " + student.EnrollmentDate);

                foreach (var enrollment in student.Enrollments)
                {

                    Console.WriteLine("Grade: " + enrollment.Grade + " Course Title: " + enrollment.Course.Title);

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

            return first.ToList();
        }

        public static IEnumerable<TFirst> Map<TFirst, TSecond, TThird, TKey>(
            this SqlMapper.GridReader reader,
            Func<TFirst, TKey> firstKey,
            Func<TSecond, TKey> secondKey,
            Func<TThird, TKey> thirdKey,
            Action<TFirst, IEnumerable<TSecond>> addChildren,
            Action<TFirst, IEnumerable<TThird>> addChildrenThird
            )
        {
            var first = reader.Read<TFirst>().ToList();

            var firstMap = reader
                .Read<TSecond>()
                .GroupBy(secondKey)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());
            var secondMap = reader
                .Read<TThird>()
                .GroupBy(thirdKey)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var item in first)
            {
                IEnumerable<TSecond> second;
                IEnumerable<TThird> third;

                if (firstMap.TryGetValue(firstKey(item), out second))
                {
                    addChildren(item, second);
                }
                if (secondMap.TryGetValue(firstKey(item), out third))
                {
                    addChildrenThird(item, third);
                }
            }

            return first;
        }
    }
}
