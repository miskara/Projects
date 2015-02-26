/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package database;

import java.io.Serializable;
import javax.persistence.*;

/**
 *
 * @author miskara
 */
@Entity
@Table(name="Sisaltaa")
public class Sisaltaa {
    
    
    private int drinkid;
    private int aineid;
    private int maara;
    private int id;

    public Sisaltaa() {
    }

    public Sisaltaa(int drinkid, int aineid, int maara, int id) {
        this.drinkid = drinkid;
        this.aineid = aineid;
        this.maara = maara;
        this.id=id;
    }
    
    @Id
    @GeneratedValue
    @Column(name="Id")
    public int getId(){
        return id;
    }
    
    public void setId(int id){
            this.id=id;
    }
    @Column(name="Drinkid")
    public int getDrinkid() {
        return drinkid;
    }

    public void setDrinkid(int drinkid) {
        this.drinkid = drinkid;
    }
    @Column(name="Aineid")
    public int getAineid() {
        return aineid;
    }

    public void setAineid(int aineid) {
        this.aineid = aineid;
    }
    @Column(name="Maara")
    public int getMaara() {
        return maara;
    }

    public void setMaara(int maara) {
        this.maara = maara;
    }
    
    
}
