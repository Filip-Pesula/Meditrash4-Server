package testMeditrash;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import java.io.IOException;
import java.io.InputStream;
import java.io.PrintWriter;
import java.io.StringReader;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import java.io.*;
import java.net.Socket;
import java.nio.charset.StandardCharsets;

public class testMeditrash {

    public static void main(String[] ars){
        try {


            DocumentBuilderFactory df = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = df.newDocumentBuilder();
            System.out.println("<Login><name>root</name><password>root</password></Login>");
            String retMessage = sendReadMsg(
                    "<Login><name>root</name><password>root</password></Login>"
            );
            Document returnMessage = db.parse(new InputSource(new StringReader(retMessage)));
            String token = returnMessage.getElementsByTagName("uniqueToken").item(0).getTextContent();
            System.out.println(token);
            System.out.println();
            XElement rootAddTrash = new XElement("Request",
                    new XElement("uniqueToken",token),
                    new XElement("requestCommand",
                            new XElement("trash",
                                    new XElement("name","jehla"),
                                    new XElement("cathegory","180101"),
                                    new XElement("weight","5")
                            ),
                            new XElement("trash",
                                    new XElement("name","naplast"),
                                    new XElement("cathegory","180104"),
                                    new XElement("weight","50")
                            ),
                            new XElement("trash",
                                    new XElement("name","obvaz"),
                                    new XElement("cathegory","170104"),
                                    new XElement("weight","5")
                            )
                    )
            );

            rootAddTrash.children.get(1).setAttribute("name","addItem");
            System.out.println( XmlWriter.createDoc(rootAddTrash));
            String retAddTrashMessage = sendReadMsg(
                    XmlWriter.createDoc(rootAddTrash)
            );
            System.out.println();
            XElement rootAddDep = new XElement("Request",
                    new XElement("uniqueToken",token),
                    new XElement("requestCommand",
                        new XElement("name","JIP")));
            rootAddDep.children.get(1).setAttribute("name","addDepartment");
            System.out.println( XmlWriter.createDoc(rootAddDep));
            String retAddDepMessage = sendReadMsg(
                    XmlWriter.createDoc(rootAddDep)
            );
            System.out.println();
            XElement rootAddUser = new XElement("Request",
                    new XElement("uniqueToken",token),
                    new XElement("requestCommand",
                        new XElement("department","JIP"),
                        new XElement("name","Josef' brambórař"),
                        new XElement("password", "I love Onions and how they feel on my skin"),
                        new XElement("rodCislo", "47854214"),
                        new XElement("rights","1"),
                        new XElement("firstName", "Josef'"),
                        new XElement("lastName", "brambó'); drop schema if exists db0;")
                    )
            );
            rootAddUser.children.get(1).setAttribute("name","addUser");
            System.out.println( XmlWriter.createDoc(rootAddUser));
            String retAddUserMessage = sendReadMsg(
                    XmlWriter.createDoc(rootAddUser)
            );
            System.out.println();
            XElement rootLoginJosef = new XElement("Login",
                    new XElement("name","Josef' brambórař"),
                    new XElement("password","I love Onions and how they feel on my skin")
            );
            System.out.println( XmlWriter.createDoc(rootLoginJosef));
            String retLoginJosef = sendReadMsg(
                    XmlWriter.createDoc(rootLoginJosef)
            );
            DocumentBuilder db2 = df.newDocumentBuilder();
            Document returnMessageLoginJosef = db2.parse(new InputSource(new StringReader(retLoginJosef)));
            String tokenLoginJosef = returnMessageLoginJosef.getElementsByTagName("uniqueToken").item(0).getTextContent();
            System.out.println();

            XElement rootAddFawItems = new XElement("Request",
                    new XElement("uniqueToken",tokenLoginJosef),
                    new XElement("requestCommand",
                            new XElement("id","1"),
                            new XElement("id","3")
                    )
            );
            rootAddFawItems.children.get(1).setAttribute("name","addFavItem");
            System.out.println( XmlWriter.createDoc(rootAddFawItems));
            String retAddFawItems = sendReadMsg(
                    XmlWriter.createDoc(rootAddFawItems)
            );
            System.out.println();

            XElement rootGetFawItems = new XElement("Request",
                    new XElement("uniqueToken",tokenLoginJosef),
                    new XElement("requestCommand")
            );
            rootGetFawItems.children.get(1).setAttribute("name","getFavList");
            System.out.println( XmlWriter.createDoc(rootGetFawItems));
            String retGetFawItems = sendReadMsg(
                    XmlWriter.createDoc(rootGetFawItems)
            );
            System.out.println();

            XElement rootGetItems = new XElement("Request",
                    new XElement("uniqueToken",tokenLoginJosef),
                    new XElement("requestCommand")
            );
            rootGetItems.children.get(1).setAttribute("name","getItems");
            System.out.println( XmlWriter.createDoc(rootGetItems));
            String retGetItems = sendReadMsg(
                    XmlWriter.createDoc(rootGetItems)
            );
            System.out.println();

            XElement rootGetCath = new XElement("Request",
                    new XElement("uniqueToken",tokenLoginJosef),
                    new XElement("requestCommand")
            );
            rootGetCath.children.get(1).setAttribute("name","getCathegories");
            System.out.println( XmlWriter.createDoc(rootGetCath));
            String retGetCath = sendReadMsg(
                    XmlWriter.createDoc(rootGetCath)
            );
            System.out.println();

            XElement rootTrashItem = new XElement("Request",
                    new XElement("uniqueToken",tokenLoginJosef),
                    new XElement("requestCommand",
                            new XElement("id","1"),
                            new XElement("count","10")
                    )
            );
            rootTrashItem.children.get(1).setAttribute("name","trashItem");
            System.out.println( XmlWriter.createDoc(rootTrashItem));
            String retTrashItem = sendReadMsg(
                    XmlWriter.createDoc(rootTrashItem)
            );
            System.out.println();

            XElement rootTrashItem1 = new XElement("Request",
                    new XElement("uniqueToken",tokenLoginJosef),
                    new XElement("requestCommand",
                            new XElement("id","1"),
                            new XElement("count","5")
                    )
            );
            rootTrashItem1.children.get(1).setAttribute("name","trashItem");
            XElement rootTrashItem2 = new XElement("Request",
                    new XElement("uniqueToken",tokenLoginJosef),
                    new XElement("requestCommand",
                            new XElement("id","1"),
                            new XElement("count","3")
                    )
            );
            rootTrashItem2.children.get(1).setAttribute("name","trashItem");
            sendReadMsg(
                    XmlWriter.createDoc(rootTrashItem1)
            );
            sendReadMsg(
                    XmlWriter.createDoc(rootTrashItem2)
            );

            XElement rootGetTrashItem = new XElement("Request",
                    new XElement("uniqueToken",tokenLoginJosef),
                    new XElement("requestCommand",
                            new XElement("yearStart","2000"),
                            new XElement("monthStart","10"),
                            new XElement("dayStart","1"),
                            new XElement("yearEnd","2022"),
                            new XElement("monthEnd","1"),
                            new XElement("dayEnd","1")
                    )
            );
            rootGetTrashItem.children.get(1).setAttribute("name","getTrashItem");
            System.out.println( XmlWriter.createDoc(rootGetTrashItem));
            String retGetTrashItem = sendReadMsg(
                    XmlWriter.createDoc(rootGetTrashItem)
            );
            System.out.println();

        } catch (IOException | ParserConfigurationException | SAXException | TransformerException e) {
            e.printStackTrace();
        }

    }
    public static String sendReadMsg(String msg) throws IOException {
        Socket clientSocket;
        PrintWriter out;
        InputStream in;
        try {
            clientSocket = new Socket("localhost", 16246);
            out = new PrintWriter(clientSocket.getOutputStream(), true);
            in = clientSocket.getInputStream();
            out.println(msg);
            while(in.available()==0){

            }
            int av = in.available();
            byte[] buf = new byte[av];
            in.read(buf,0,av);
            String resp = new String(buf, StandardCharsets.UTF_8);

            System.out.println(resp);

            while (!clientSocket.isConnected()){

            }
            return resp;
        }
        catch (IOException e) {
            e.printStackTrace();
        }
        return "";
    }


}
