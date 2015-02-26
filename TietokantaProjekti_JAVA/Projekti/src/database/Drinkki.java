/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package database;

/**
 *
 * @author miskara
 */
import javax.persistence.*;
import java.util.*;

/**
 *
 * @author miskara
 */
@Entity
@Table(name="Drinkki")
public class Drinkki {
    
    private int id;
    private String nimi;
    private Double hinta;

    public Drinkki() {
    }

    public Drinkki(int id, String nimi, Double hinta) {
        this.id = id;
        this.nimi = nimi;
        this.hinta = hinta;
    }
    @Id
    @GeneratedValue
    @Column(name="Id")
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }
    @Column(name="Nimi")
    public String getNimi() {
        return nimi;
    }

    public void setNimi(String nimi) {
        this.nimi = nimi;
    }
    @Column(name="Hinta")
    public Double getHinta() {
        return hinta;
    }

    public void setHinta(Double hinta) {
        this.hinta = hinta;
    }
    
    
}
