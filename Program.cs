using System;

namespace AlgorithmsProject
{
    // استخدام enum للتقديرات
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

        // مؤشرات اللائحة الخطية المزدوجة
        public Student Next;
        public Student Prev;

        public Student(int id, string name, string city, double e1, double e2)
        {
            StudentID = id;
            StudentName = name;
            CityName = city;
            FirstExam = e1;
            SecondExam = e2;
            // حسبة المحصلة بجمع علامتي الاختبار وقسمتها على 2
            FinalResult = (e1 + e2) / 2;
            
            // توزيع التقدير حسب المحصلة
            if (FinalResult >= 90) StudentGrade = GradeStatus.Excellent;
            else if (FinalResult >= 80) StudentGrade = GradeStatus.VeryGood;
            else if (FinalResult >= 50) StudentGrade = GradeStatus.Good;
            else StudentGrade = GradeStatus.Fail;
        }
    }

    class Program
    {
        static Student head = null;
        static Student tail = null;

        static void Main(string[] args)
        {
            // رسائل ترحيبية للمستخدم العادي
            Console.WriteLine("=== نظام إدارة سجلات الطلاب ===");

            // إدخال بيانات 5 طلاب كبداية
            int count = 1;
            while (count <= 5)
            {
                Console.WriteLine($"\n--- تسجيل الطالب {count} من 5 ---");
                if (InputStudentData(false)) count++;
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n1. عرض الكل | 2. فرز | 3. بحث (عودي) | 4. إضافة | 5. حذف | 6. متفوقين | 0. خروج");
                Console.Write("اختر: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": DisplayAll(); break;
                    case "2": SortMenu(); break;
                    case "3": SearchMenu(); break;
                    case "4": AddByChoice(); break;
                    case "5": DeleteByID(); break; // حذف طالب برقم محدد
                    case "6": ShowHighAchievers(); break; // أعلى من 85
                    case "0": exit = true; break;
                }
            }
        }

        static bool InputStudentData(bool isManualAdd)
        {
            try
            {
                Console.Write("الرقم: "); int id = int.Parse(Console.ReadLine());
                Console.Write("الاسم: "); string name = Console.ReadLine();
                Console.Write("المحافظة: "); string city = Console.ReadLine();
                Console.Write("الاختبار 1: "); double e1 = double.Parse(Console.ReadLine());
                Console.Write("الاختبار 2: "); double e2 = double.Parse(Console.ReadLine());

                Student s = new Student(id, name, city, e1, e2);
                if (!isManualAdd) InsertAtEnd(s);
                return true;
            }
            catch { Console.WriteLine("خطأ في البيانات!"); return false; }
        }

        // إضافة في النهاية
        static void InsertAtEnd(Student s)
        {
            if (head == null) head = tail = s;
            else { tail.Next = s; s.Prev = tail; tail = s; s.Next = null; }
        }

        // إضافة في البداية
        static void InsertAtStart(Student s)
        {
            if (head == null) head = tail = s;
            else { s.Next = head; head.Prev = s; head = s; s.Prev = null; }
        }

        static void DisplayAll()
        {
            Student temp = head;
            while (temp != null)
            {
                Console.WriteLine($"{temp.StudentID} | {temp.StudentName} | {temp.FinalResult} | {temp.StudentGrade}");
                temp = temp.Next;
            }
        }

        // قائمة الفرز
        static void SortMenu()
        {
            Console.WriteLine("1. بالاسم (A-Z) | 2. بالمحصلة (أدنى للأعلى)");
            string c = Console.ReadLine();
            BubbleSort(c == "1");
        }

        static void BubbleSort(bool byName)
        {
            if (head == null) return;
            bool swapped;
            do {
                swapped = false;
                Student curr = head;
                while (curr.Next != null) {
                    // شروط الفرز المطلوبة
                    bool condition = byName ? 
                        string.Compare(curr.StudentName, curr.Next.StudentName) > 0 : 
                        curr.FinalResult > curr.Next.FinalResult;

                    if (condition) { SwapData(curr, curr.Next); swapped = true; }
                    curr = curr.Next;
                }
            } while (swapped);
        }

        static void SwapData(Student a, Student b)
        {
            // تبديل كل الحقول لضمان عدم ضياع البيانات
            int tId = a.StudentID; a.StudentID = b.StudentID; b.StudentID = tId;
            string tN = a.StudentName; a.StudentName = b.StudentName; b.StudentName = tN;
            double tR = a.FinalResult; a.FinalResult = b.FinalResult; b.FinalResult = tR;
            GradeStatus tG = a.StudentGrade; a.StudentGrade = b.StudentGrade; b.StudentGrade = tG;
        }

        // البحث العودي (Recursive)
        static void SearchMenu()
        {
            Console.Write("أدخل العلامة للبحث: ");
            double v = double.Parse(Console.ReadLine());
            RecursiveSearch(head, v);
        }

        static void RecursiveSearch(Student node, double target)
        {
            if (node == null) return;
            if (node.FirstExam == target || node.SecondExam == target)
                Console.WriteLine($"تم العثور: {node.StudentName}");
            RecursiveSearch(node.Next, target);
        }

        // حذف طالب
        static void DeleteByID()
        {
            Console.Write("ID للحذف: ");
            int id = int.Parse(Console.ReadLine());
            Student curr = head;
            while (curr != null) {
                if (curr.StudentID == id) {
                    if (curr.Prev != null) curr.Prev.Next = curr.Next; else head = curr.Next;
                    if (curr.Next != null) curr.Next.Prev = curr.Prev; else tail = curr.Prev;
                    return;
                }
                curr = curr.Next;
            }
        }

        static void ShowHighAchievers()
        {
            Student t = head;
            while (t != null) {
                if (t.FinalResult > 85) Console.WriteLine(t.StudentName);
                t = t.Next;
            }
        }

        static void AddByChoice()
        {
            Console.WriteLine("1. في البداية | 2. في النهاية");
            string c = Console.ReadLine();
            // استدعاء دالة الإدخال ثم التوجيه
            if (InputStudentData(true)) {
                Student s = tail; // آخر مضاف
                if (c == "1") InsertAtStart(s); 
            }
        }
    }
}