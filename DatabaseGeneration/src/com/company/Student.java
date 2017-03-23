package com.company;

import java.util.HashMap;

/**
 * Created by nathanliu on 23/03/2017.
 */

public class Student {

    private int id;
    private int schoolID;
    private int gender;

    private double attendence;
    private int detentions;
    private int merit;
    private int distinctions;
    private int musicalInstrument;

    private int gcseYear;

    private HashMap<Helpers.Grades, Integer> grades;

    public Student(int id, int schoolID) {
        this.id = id;
        this.schoolID = schoolID;
    }

    public int getId() {
        return id;
    }

    public int getSchoolID() {
        return schoolID;
    }

    public int getGender() {
        return gender;
    }

    public double getAttendence() {
        return attendence;
    }

    public int getDetentions() {
        return detentions;
    }

    public int getMerit() {
        return merit;
    }

    public int getDistinctions() {
        return distinctions;
    }

    public int getMusicalInstrument() {
        return musicalInstrument;
    }

    public int getGcseYear() {
        return gcseYear;
    }

    public HashMap<Helpers.Grades, Integer> getGrades() {
        return grades;
    }
}
