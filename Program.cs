using System;


namespace AlgorithmsProject
{
       //  (Enum) تعريف التقديرات المتاحة حسب طلب المسألة
    enum GradeStatus { Fail, Good, VeryGood, Excellent }

    class Student
    {
        public int StudentID;
        public string StudentName;
        public string CityName;
        public double FirstExam;
        public double SecondExam;
        public double FinalResult;
        public GradeStatus StudentGrade;

                //  مؤشرات اللائحة الخطية المزدوجة للربط الأمامي والخلفي
        public Student Next;
        public Student Prev;

        public Student(int id, string name, string city, double e1, double e2)
        {
            StudentID = id;
            StudentName = name;
            CityName = city;
            FirstExam = e1;
            SecondExam = e2;

                  //  حساب المحصلة: (الاختبار 1 + الاختبار 2) / 2
            FinalResult = (e1 + e2) / 2;

                //  تحديد التقدير الأكاديمي بناءً على المحصلة المحسوبة
            if (FinalResult >= 90) StudentGrade = GradeStatus.Excellent;
            else if (FinalResult >= 80) StudentGrade = GradeStatus.VeryGood;
            else if (FinalResult >= 60) StudentGrade = GradeStatus.Good;
            else StudentGrade = GradeStatus.Fail;
        }
    }

    class Program
    {
        static Student head = null;
        static Student tail = null;

        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   STUDENT MANAGEMENT SYSTEM (ALGORITHMS)   ");
            Console.WriteLine("========================================");

                 //  إدخال بيانات 5 طلاب بشكل إجباري عند بدء التشغيل كما هو مطلوب
            int count = 1;
            while (count <= 5)
            {
                Console.WriteLine($"\n>>> Enter data for Student ({count}/5):");
                Student s = CreateStudent();
                if (s != null)
                {
                    InsertAtEnd(s);
                    Console.WriteLine($"[System]: {s.StudentName} added. Avg: {s.FinalResult}, Grade: {s.StudentGrade}");
                    count++;
                }
            }
            
            bool exit = false;
            while (!exit)
            {
             //  القائمة الرئيسية الموجهة للمستخدم العادي باللغة الإنكليزية
                Console.WriteLine("\n----------------------------------------");
                Console.WriteLine("MAIN MENU:");
                Console.WriteLine("1. Display Registered Students");
                Console.WriteLine("2. Sort Students (Name/Grade)");
                Console.WriteLine("3. Recursive Search for a Score");
                Console.WriteLine("4. Add a New Student");
                Console.WriteLine("0. Exit Program");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": DisplayAll(); break;
                    case "2": SortMenu(); break;
                    case "3": SearchMenu(); break;
                    case "4": AddByChoice(); break;
                    case "0": exit = true; Console.WriteLine("Closing System..."); break;
                    default: Console.WriteLine("Invalid option, try again."); break;
                }
            }
        }

                   //  دالة لقراءة بيانات الطالب من الكونسول مع معالجة الأخطاء
        static Student CreateStudent()
        {
            try
            {
                Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
                Console.Write("Full Name: "); string name = Console.ReadLine();
                Console.Write("Province: "); string city = Console.ReadLine();
                Console.Write("Exam 1: "); double e1 = double.Parse(Console.ReadLine());
                Console.Write("Exam 2: "); double e2 = double.Parse(Console.ReadLine());

                return new Student(id, name, city, e1, e2);
            }
            catch
            {
                Console.WriteLine("(!) Input error: Please enter valid numbers.");
                return null;
            }
        }

        // إضافة عقدة جديدة في نهاية اللائحة المزدوجة
        static void InsertAtEnd(Student s)
        {
            if (head == null) { head = tail = s; s.Next = s.Prev = null; }
            else { tail.Next = s; s.Prev = tail; tail = s; s.Next = null; }
        }

              //  إضافة عقدة جديدة في بداية اللائحة المزدوجة
        static void InsertAtStart(Student s)
        {
            if (head == null) { head = tail = s; s.Next = s.Prev = null; }
            else { s.Next = head; head.Prev = s; head = s; s.Prev = null; }
        }

        // عرض كافة البيانات المخزنة في اللائحة حالياً
        static void DisplayAll()
        {
            Student temp = head;
            if (temp == null) { Console.WriteLine("List is empty."); return; }
            Console.WriteLine("\n--- Student Records ---");
            while (temp != null)
            {
                Console.WriteLine($"ID: {temp.StudentID} | Name: {temp.StudentName} | City: {temp.CityName} | Avg: {temp.FinalResult} | Grade: {temp.StudentGrade}");
                temp = temp.Next;
            }
        }

                 //  قائمة اختيار نوع الفرز (حسب الاسم أو المحصلة)
        static void SortMenu()
        {
            Console.WriteLine("\nSort by:");
            Console.WriteLine("1. Name (A to Z)");
            Console.WriteLine("2. Score (Low to High)");
            Console.Write("Choice: ");
            string c = Console.ReadLine();
            if (c == "1") BubbleSort(true);
            else if (c == "2") BubbleSort(false);
        }

        // تنفيذ خوارزمية الفقاعة لترتيب اللائحة المزدوجة
        static void BubbleSort(bool byName)
        {
            if (head == null) return;
            bool swapped;
            do
            {
                swapped = false;
                Student curr = head;
                while (curr.Next != null)
                {
                    bool condition = byName ?
                        string.Compare(curr.StudentName, curr.Next.StudentName) > 0 :
                        curr.FinalResult > curr.Next.FinalResult;

                    if (condition) { SwapData(curr, curr.Next); swapped = true; }
                    curr = curr.Next;
                }
            } while (swapped);
            Console.WriteLine("Sorting completed.");
        }

        // تبديل قيم الحقول بين العقد 
        static void SwapData(Student a, Student b)
        {
            int tId = a.StudentID; a.StudentID = b.StudentID; b.StudentID = tId;
            string tN = a.StudentName; a.StudentName = b.StudentName; b.StudentName = tN;
            string tC = a.CityName; a.CityName = b.CityName; b.CityName = tC;
            double te1 = a.FirstExam; a.FirstExam = b.FirstExam; b.FirstExam = te1;
            double te2 = a.SecondExam; a.SecondExam = b.SecondExam; b.SecondExam = te2;
            double tR = a.FinalResult; a.FinalResult = b.FinalResult; b.FinalResult = tR;
            GradeStatus tG = a.StudentGrade; a.StudentGrade = b.StudentGrade; b.StudentGrade = tG;
        }

        // قائمة البحث عن علامة معينة
        static void SearchMenu()
        {
            Console.Write("Enter the score to search for: ");
            if (double.TryParse(Console.ReadLine(), out double val))
            {
                Console.WriteLine($"Searching for score {val}...");
                //  استدعاء دالة البحث العودي بدءاً من أول اللائحة
                bool found = RecursiveSearch(head, val);
                if (!found) Console.WriteLine("No matches found.");
            }
        }

              //  داله البحث العودي
        static bool RecursiveSearch(Student node, double target)
        {
            if (node == null) return false;

            bool match = false;
            // معالجة دقة الأرقام العشرية عند المقارنة
            if (Math.Abs(node.FinalResult - target) < 0.01)
            {
                Console.WriteLine($">> Match Found: {node.StudentName} (ID: {node.StudentID})");
                match = true;
            }

            return RecursiveSearch(node.Next, target) || match;
        }

        //  خيار إضافة طالب جديد في موقع محدد
        static void AddByChoice()
        {
            Console.WriteLine("\nWhere to add?");
            Console.WriteLine("1. At Beginning");
            Console.WriteLine("2. At End");
            Console.Write("Selection: ");
            string c = Console.ReadLine();
            Student s = CreateStudent();
            if (s != null)
            {
                if (c == "1") InsertAtStart(s);
                else InsertAtEnd(s);
                Console.WriteLine("Student added successfully.");
            }
        }
    }
}
