/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package projekti;

import database.*;
import java.util.List;
import org.hibernate.*;
import org.hibernate.cfg.AnnotationConfiguration;
import org.hibernate.exception.DataException;

/**
 *
 * @author miskara
 */
public class Kyselyt {

    public List<Drinkki> Drinkkilista;
    public List<Ainesosa> ainelista;
    public List<Sisaltaa> sislista;

    public void avaaDB() {
        SessionFactory istuntotehdas = new AnnotationConfiguration().configure().buildSessionFactory();
        Session istunto = istuntotehdas.openSession();
        Transaction transaktio = null;
        try {
            transaktio = istunto.beginTransaction();

            String drinkki = "from Drinkki";
            String aine = "from Ainesosa";
            String sisaltaa = "from Sisaltaa";
            Drinkkilista = istunto.createQuery(drinkki).list();
            ainelista = istunto.createQuery(aine).list();
            sislista = istunto.createQuery(sisaltaa).list();

            transaktio.commit();
        } catch (Exception e) {
            if (transaktio != null) {
                if (transaktio.isActive()) {
                    try {
                        transaktio.rollback();
                        System.out.println("ROLLBACKI TEHTY");
                    } catch (HibernateException e1) {
                        System.err.println("Tapahtuman peruutus epÃ¤onnistui.");
                    }

                }
            }
            e.printStackTrace();
        } finally {
            istunto.close();
        }
    }
    
    public void poistaAinesosa(String aine){
        
    }
    
    public void lisääAinesosa(String aine, int varastoon) {
        SessionFactory istuntotehdas = new AnnotationConfiguration().configure().buildSessionFactory();
        Session istunto = istuntotehdas.openSession();
        Transaction transaktio = null;
        try {
            transaktio = istunto.beginTransaction();

            Ainesosa a = new Ainesosa(0, aine, varastoon);
            istunto.saveOrUpdate(a);
            transaktio.commit();
        } catch (Exception e) {
            if (transaktio != null) {
                if (transaktio.isActive()) {
                    try {
                        transaktio.rollback();
                        System.out.println("ROLLBACKI TEHTY");
                    } catch (HibernateException e1) {
                        System.err.println("Tapahtuman peruutus epÃ¤onnistui.");
                    }

                }
            }
            e.printStackTrace();
        } finally {
            istunto.close();
        }
    }

    public void ostaJuoma(int item) {
        SessionFactory istuntotehdas = new AnnotationConfiguration().configure().buildSessionFactory();
        Session istunto = istuntotehdas.openSession();
        Transaction transaktio = null;
        try {
            transaktio = istunto.beginTransaction();
            String maarat = "from Sisaltaa where Drinkid= :did";
            int d_id = Drinkkilista.get(item).getId();
            Query q1 = istunto.createQuery(maarat);
            q1.setParameter("did", d_id);
            List<Sisaltaa> maaralista = q1.list();

            for (Sisaltaa s : maaralista) {
                String update = "UPDATE Ainesosa set Varastossa = Varastossa-" + s.getMaara() + " where id = " + s.getAineid();
                Query q = istunto.createQuery(update);
                int result = q.executeUpdate();
                System.out.println("Rows affected: " + result);
            }

            transaktio.commit();
        }catch (DataException de){
            System.err.println("Varastossa ei ole tarpeeksi ainesosia");
        }  catch (Exception e) {
            if (transaktio != null) {
                if (transaktio.isActive()) {
                    try {
                        transaktio.rollback();
                        System.out.println("ROLLBACKI TEHTY");
                    } catch (HibernateException e1) {
                        System.err.println("Tapahtuman peruutus epÃ¤onnistui.");
                    }

                }
            }
            e.printStackTrace();
        } finally {
            istunto.close();
        }
    }

    public void tilaus(String nimi, int määrä) {
        SessionFactory istuntotehdas = new AnnotationConfiguration().configure().buildSessionFactory();
        Session istunto = istuntotehdas.openSession();
        Transaction transaktio = null;
        try {
            transaktio = istunto.beginTransaction();
            String update = "UPDATE Ainesosa set Varastossa = Varastossa+:maara where nimi = :nimi";
            Query q = istunto.createQuery(update);
            q.setParameter("maara", määrä   );
            q.setParameter("nimi", nimi);
            int result = q.executeUpdate();
            System.out.println("Rows affected: " + result);
            transaktio.commit();
        } 
        catch (Exception e) {
            if (transaktio != null) {
                if (transaktio.isActive()) {
                    try {
                        transaktio.rollback();
                        System.out.println("ROLLBACKI TEHTY");
                    } catch (HibernateException e1) {
                        System.err.println("Tapahtuman peruutus epÃ¤onnistui.");
                    }

                }
            }
            e.printStackTrace();
        } finally {
            istunto.close();
        }
    }
    
    public void lisääDrinkki(String drinkki,double hinta,int[] ainesosat,int[] maara){
    SessionFactory istuntotehdas = new AnnotationConfiguration().configure().buildSessionFactory();
        Session istunto = istuntotehdas.openSession();
        Transaction transaktio = null;
        try {
            transaktio = istunto.beginTransaction();
            
            Drinkki d = new Drinkki(0, drinkki, hinta);
            Sisaltaa[] s = new Sisaltaa[6];
            int[] a = ainesosat;

            istunto.saveOrUpdate(d);
            for(int i = 0; i < ainesosat.length; i++){
                s[i] = new Sisaltaa(d.getId(), a[i]+1, maara[i], 0);
            }

           
           for(int i = 0; i < ainesosat.length; i++){
           istunto.saveOrUpdate(s[i]);
           }
            transaktio.commit();
        } catch (Exception e) {
            if (transaktio != null) {
                if (transaktio.isActive()) {
                    try {
                        transaktio.rollback();
                        System.out.println("ROLLBACKI TEHTY");
                    } catch (HibernateException e1) {
                        System.err.println("Tapahtuman peruutus epÃ¤onnistui.");
                    }

                }
            }
            e.printStackTrace();
        } finally {
            istunto.close();
        }
}


public String[] naytaAinesosat(int drinkki){
    SessionFactory istuntotehdas = new AnnotationConfiguration().configure().buildSessionFactory();
        Session istunto = istuntotehdas.openSession();
        Transaction transaktio = null;
        try {
            transaktio = istunto.beginTransaction();
            
            
           String maarat = "from Sisaltaa where Drinkid= :did";
           int d_id = Drinkkilista.get(drinkki).getId();
           Query q1 = istunto.createQuery(maarat);
           q1.setParameter("did", d_id);
           List<Sisaltaa> maaralista = q1.list();

           
           Ainesosa[] a = new Ainesosa[6];
           int i = 0;
           int a_id;
           for(Sisaltaa s : maaralista){
           
            
           String aineet = "from Ainesosa where id = :aid";
           a_id = s.getAineid();
           Query q = istunto.createQuery(aineet).setParameter("aid", a_id);
       
           a[i] = (Ainesosa)q.uniqueResult();
           
           i++;
           }

          Integer k;
          String s[] = new String[a.length];  
          for(int j = 0; j < maaralista.size(); j++){
              k = maaralista.get(j).getMaara();
             
              s[j] = a[j].getNimi()+" "+k.toString()+" cl";
          }
            System.out.println(s);
           
            transaktio.commit();
            
            return s;
        } catch (Exception e) {
            if (transaktio != null) {
                if (transaktio.isActive()) {
                    try {
                        transaktio.rollback();
                        System.out.println("ROLLBACKI TEHTY");
                    } catch (HibernateException e1) {
                        System.err.println("Tapahtuman peruutus epÃ¤onnistui.");
                    }

                }
            }
            e.printStackTrace();
            return null;
        } finally {
            istunto.close();
            
            
            
        }
}
}
