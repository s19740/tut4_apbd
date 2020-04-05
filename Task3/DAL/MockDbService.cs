using System.Collections.Generic;
using Task3.Models;
using Microsoft.Data.SqlClient;

namespace Task3.DAL {
    public class MockDbService : IDbService {

        private static IEnumerable<Student> _students;
        //Data Source=db-mssql;Initial Catalog=s19740;Integrated Security=True
        private static SqlConnection sqlConnection;
        static MockDbService() {
                  sqlConnection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19740;Integrated Security=True");
            
            /* students = new List<Student> {
                new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski"},
                new Student{IdStudent=2, FirstName="Anna", LastName="Malewski"},
                new Student{IdStudent=3, FirstName="Andrzej", LastName="Kowalski"}
            };*/
        }
        public IEnumerable<Student> GetStudents() {
            var students = new List<Student>();
            using(sqlConnection){//connection string, you have to find yours
                using(var command = new SqlCommand()){
                    command.Connection = sqlConnection;
                    command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, st.Name as Studies,e.Semester from Student s join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy";
                    sqlConnection.Open();
                    var reader = command.ExecuteReader();

                    while(reader.Read()){
                        var stud = new Student();
                        stud.FirstName = reader["FirstName"].ToString();
                        stud.LastName = reader["LastName"].ToString();
                        stud.BirthDate = DateTime.Parse(reader["BirthDate"].ToString());
                        stud.Studies = reader["Studies"].ToString();
                        stud.Semester = int.Parse(reader["Semester"].ToString());
                        students.Add(stud);
                    }
                }
            }
            return students;
        }
        public List<string> GetSemesterEntries(string studentId){
            var entries = new List<string>();
            using(sqlConnection){
                using(var command = new SqlCommand()){
                    command.Connection = sqlConnection;
                    command.CommandText = "select Semester from Enrollment, Student where Student.IdEnrollment = Enrollment.IdEnrollment AND Student.IndexNumber = @studentId;";
                    command.Parameters.AddWithValue("studentId", studentId);
                    sqlConnection.Open();
                    var reader = command.ExecuteReader();
                    while(reader.Read()){
                        semesterEntries.Add(reader["Semester"].ToString());
                    }
                }
            }
            return entries;
        }
    }
    }
