package com.company;

import java.util.HashMap;
import java.util.List;

/**
 * Created by nathanliu on 23/03/2017.
 */
public class School {

    private int id;
    private String name;

    private int population;
    private int isPublic;
    private String region;
    private String city;

    public School(int id, String name, int population, int isPublic, String region, String city) {
        this.id = id;
        this.name = name;

        this.population = population;
        this.isPublic = isPublic;
        this.region = region;
        this.city = city;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public int getPopulation() {
        return population;
    }

    public int getIsPublic() {
        return isPublic;
    }

    public String getRegion() {
        return region;
    }

    public String getCity() {
        return city;
    }
}
