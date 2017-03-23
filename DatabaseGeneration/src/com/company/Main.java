package com.company;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class Main {

    public static void main(String[] args) {

        Generator.ReadFile();
        System.out.println("Finished Reading File");

        Generator.GenerateSchools();
        Generator.GenerateSchoolGrades();
        Generator.GenerateStudents();
        Generator.GenerateStudentStatistics();
        Generator.GenerateStudentGrades();

    }
}




