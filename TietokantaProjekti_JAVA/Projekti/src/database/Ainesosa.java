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

@Entity
@Table(name="Ainesosa")
public class Ainesosa {
    
    private int id;
    private String nimi;
    private int varastossa;

    public Ainesosa() {
    }

    public Ainesosa(int id, String nimi, int varastossa) {
        this.id = id;
        this.nimi = nimi;
        this.varastossa = varastossa;
    }
    @Id
    @GeneratedValue
    @Column(name="ID")
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
    @Column(name="Varastossa")
    public int getVarastossa() {
        return varastossa;
    }

    public void setVarastossa(int varastossa) {
        this.varastossa = varastossa;
    }
    
    
}
