package com.company;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

/**
 * Created by nathanliu on 23/03/2017.
 */
public class Helpers {

    public static enum Grades {
        AStar, A, B, C, D, E, U
    }

    private static Random rand = new Random();

    public static int GenerateRandomInteger(int minVal, int maxValue) {
        return rand.nextInt((maxValue - minVal) + 1) + minVal;
    }

    public static double GenerateRandomDouble(double minVal, double maxVal) {
        return minVal + (maxVal - minVal) * rand.nextDouble();
    }

    public static List<Integer> Generate_N_Integers(int n, int maxVal) {

        List<Integer> outputList = new ArrayList<>();

        for (int i = 0; i < n - 1; i++) {

            int generatedValue = GenerateRandomInteger(0, maxVal);
            outputList.add(generatedValue);
            maxVal -= generatedValue;

        }

        outputList.add(maxVal);

        return outputList;

    }

    public static List<Double> Generate_N_Decimals(int n) {

        List<Double> outputList = new ArrayList<>();

        double maxVal = 1;

        for (int i = 0; i < n - 1; i++) {

            double generatedValue = 0 + (maxVal - 0) * rand.nextDouble();
            outputList.add(generatedValue);
            maxVal -= generatedValue;

        }

        outputList.add(maxVal);

        return outputList;

    }

}
