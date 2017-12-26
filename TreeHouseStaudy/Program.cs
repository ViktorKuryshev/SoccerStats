using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeHouseStaudy
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Student> students = new List<Student>()
			{
				new Student() {Name = "Sali", GradeLevel = 3 },
				new Student() {Name = "Bob", GradeLevel = 2 },
				new Student() {Name = "Sali", GradeLevel = 2 },
				new Student() {Name = "Mat", GradeLevel = 4 },
				
			};

			students.Sort();

			foreach(Student student in students)
			{
				Console.WriteLine($"{student.Name} is in grade {student.GradeLevel}");
			}
		}
	}
}
