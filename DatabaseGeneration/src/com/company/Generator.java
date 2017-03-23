package com.company;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by nathanliu on 23/03/2017.
 */
public class Generator {

    private static String[] cities = {"Oxford", "London", "Cambridge", "Bristol", "Swindon", "Manchester", "Liverpool"};

    private static List<String> inputList = new ArrayList<>();
    private static List<School> schoolList = new ArrayList<>();
    private static List<Student> studentList = new ArrayList<>();

    public static void GenerateSchools() {

        List<String> SQL_statements_school = new ArrayList<>();

        for (int i = 1; i < inputList.size(); i++) {

            String selectedCity = cities[Helpers.GenerateRandomInteger(0, cities.length - 1) ];
            int isPublic = Helpers.GenerateRandomInteger(0,1);
            int population = Helpers.GenerateRandomInteger(30,200);

            schoolList.add(new School(i, inputList.get(i), population, isPublic, "SE", selectedCity));

            SQL_statements_school.add("insert into schools.school values (" + i + ", \"" + inputList.get(i) + "\", "
                    + population + ", " + isPublic + ", \"SE\", \"" + selectedCity + "\");");
        }

        System.out.println("Finished Generating Schools");
        String outputPath = System.getProperty("user.dir") + "/Resources/SQL Files/school_table.txt";
        WriteTofile(outputPath, SQL_statements_school);

    }

    public static void GenerateSchoolGrades() {

        List<String> SQL_statements_schoolGrades = new ArrayList<>();

        for (School school : schoolList) {

            for (int i = 2010; i < 2017; i++) {

                List<Double> gradeStatistics = Helpers.Generate_N_Decimals(7);

                SQL_statements_schoolGrades.add("insert into schools.grades values (" + school.getId() + ", \"" + i + "\", "
                        + gradeStatistics.get(0) + ", "
                        + gradeStatistics.get(1) + ", "
                        + gradeStatistics.get(2) + ", "
                        + gradeStatistics.get(3) + ", "
                        + gradeStatistics.get(4) + ", "
                        + gradeStatistics.get(5) + ", "
                        + gradeStatistics.get(6) + "); ");

            }

        }

        System.out.println("Finished Generating Schools Statistics");
        String outputPath = System.getProperty("user.dir") + "/Resources/SQL Files/school_grades.txt";
        WriteTofile(outputPath, SQL_statements_schoolGrades);

    }

    public static void GenerateStudents() {

        int studentID = 0;

        List<String> SQL_statements = new ArrayList<>();

        for (School school : schoolList) {

            for (int i = 0; i < school.getPopulation(); i++) {

                int gender = Helpers.GenerateRandomInteger(0, 1);
                studentList.add(new Student(studentID, school.getId()));

                SQL_statements.add("insert into schools.students values (" + studentID++ + ", \"" + school.getId() + "\", "
                        + gender + "); ");

            }

        }

        System.out.println("Finished Generating Student IDs");
        String outputPath = System.getProperty("user.dir") + "/Resources/SQL Files/school_students.txt";
        WriteTofile(outputPath, SQL_statements);


    }

    public static void GenerateStudentStatistics() {

        List<String> SQL_statements = new ArrayList<>();

        for (Student student : studentList) {

            double attendance = Helpers.GenerateRandomDouble(0, 1);
            double detentions = Helpers.GenerateRandomInteger(0, 10);
            double distinctions = Helpers.GenerateRandomInteger(0, 10);
            double merit = Helpers.GenerateRandomInteger(0, 10);
            double instrument = Helpers.GenerateRandomInteger(0, 10);


            SQL_statements.add("insert into schools.student_stats values (" + student.getId() + ", "
                    + attendance + ", "
                    + detentions + ", "
                    + distinctions + ", "
                    + merit + ", "
                    + instrument + "); ");

        }

        System.out.println("Finished Generating Student Statistics");
        String outputPath = System.getProperty("user.dir") + "/Resources/SQL Files/school_studentsStatistics.txt";
        WriteTofile(outputPath, SQL_statements);

    }

    public static void GenerateStudentGrades() {

        List<String> SQL_statements = new ArrayList<>();

        for (Student student : studentList) {

            List<Integer> gcseGrades = Helpers.Generate_N_Integers(7, 11);

            int yearTaken = Helpers.GenerateRandomInteger(2010, 2016);

            SQL_statements.add("insert into schools.gcse values (" + student.getId() + ", "
                    + yearTaken + ", "
                    + gcseGrades.get(0) + ", "
                    + gcseGrades.get(1) + ", "
                    + gcseGrades.get(2) + ", "
                    + gcseGrades.get(3) + ", "
                    + gcseGrades.get(4) + ", "
                    + gcseGrades.get(5) + ", "
                    + gcseGrades.get(6) + "); ");

        }

        System.out.println("Finished Generating Student Grades");
        String outputPath = System.getProperty("user.dir") + "/Resources/SQL Files/school_students_grades.txt";
        WriteTofile(outputPath, SQL_statements);

    }

    private static void WriteTofile(String path, List<String> values) {

        try {

            BufferedWriter writer = new BufferedWriter(new FileWriter(path));
            for (String value : values) {
                writer.write(value);
                writer.newLine();
            }
            writer.close();

        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    public static void ReadFile() {
        String path = System.getProperty("user.dir") + "/Resources/schools.txt";


        try {
            BufferedReader br = new BufferedReader(new FileReader(path));
            String line = br.readLine();

            while (line != null) {
                inputList.add(line);
                line = br.readLine();
            }

            br.close();
        } catch (Exception e) {
            e.printStackTrace();
        }

    }

}
