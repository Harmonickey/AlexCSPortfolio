/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package hatchu;
import java.awt.*;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import javax.swing.*;

/**
 *
 * @author Alex
 */
public class Hatchu {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        final HatchUI hatchui = new HatchUI();
        hatchui.setVisible(true);
        hatchui.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        
        Dimension dim = Toolkit.getDefaultToolkit().getScreenSize();
        
        int w = hatchui.getWidth();
        int h = hatchui.getHeight();
        int x = (dim.width - w) / 2;
        int y = (dim.height - h) / 2;
        
        hatchui.setLocation(x, y);
        
        hatchui.addWindowListener(new WindowListener() {
        
            @Override
            public void windowDeactivated(WindowEvent evt) { }
         
            @Override
            public void windowActivated(WindowEvent evt) { }
            
            @Override
            public void windowDeiconified(WindowEvent evt) { }
            
            @Override
            public void windowIconified(WindowEvent evt) { }
            
            @Override
            public void windowClosed(WindowEvent evt) { 
                hatchuiFormClosing(HatchUI.shouldReset);
            }
            
            @Override
            public void windowClosing(WindowEvent evt) { }
            
            @Override
            public void windowOpened(WindowEvent evt) { }
           
        });
        
    }
    
    private static void hatchuiFormClosing(boolean shouldReset)
    {
        if (shouldReset)
        {
            
            final HatchUI hatchui = new HatchUI();
            hatchui.setVisible(true);

            Dimension dim = Toolkit.getDefaultToolkit().getScreenSize();

            int w = hatchui.getWidth();
            int h = hatchui.getHeight();
            int x = (dim.width - w) / 2;
            int y = (dim.height - h) / 2;

            hatchui.setLocation(x, y);
            
            HatchUI.shouldReset = false;

            hatchui.addWindowListener(new WindowListener() {

                @Override
                public void windowDeactivated(WindowEvent evt) { }

                @Override
                public void windowActivated(WindowEvent evt) { }

                @Override
                public void windowDeiconified(WindowEvent evt) { }

                @Override
                public void windowIconified(WindowEvent evt) { }

                @Override
                public void windowClosed(WindowEvent evt) { 
                    hatchuiFormClosing(HatchUI.shouldReset);
                }

                @Override
                public void windowClosing(WindowEvent evt) { }

                @Override
                public void windowOpened(WindowEvent evt) { }

            });
        }
        else
        {
            System.exit(0);
        }
    }
}
