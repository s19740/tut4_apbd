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
            
            /* _students = new List<Student> {
                new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski"},
                new Student{IdStudent=2, FirstName="Anna", LastName="Malewski"},
                new Student{IdStudent=3, FirstName="Andrzej", LastName="Kowalski"}
            };*/
        }
        public IEnumerable<Student> GetStudents() {
            var students = new List<Student>();
            using(_sqlConnection){//connection string, you have to find yours
                using(var command = new SqlCommand()){
                    command.Connection = _sqlConnection;
                    command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, st.Name as Studies,e.Semester from Student s join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy";
                    _sqlConnection.Open();
                    var reader = command.ExecuteReader();

                    while(reader.Read()){
                        var st = new Student();
                        st.FirstName = reader["FirstName"].ToString();
                        st.LastName = reader["LastName"].ToString();
                        st.BirthDate = DateTime.Parse(reader["BirthDate"].ToString());
                        st.Studies = reader["Studies"].ToString();
                        st.Semester = int.Parse(reader["Semester"].ToString());
                        students.Add(st);
                    }
                }
            }
            return students;
        }

    }
    }