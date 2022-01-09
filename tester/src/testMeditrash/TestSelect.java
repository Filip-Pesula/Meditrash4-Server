package testMeditrash;

import org.w3c.dom.Document;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.TransformerException;
import java.io.IOException;
import java.io.StringReader;

public class TestSelect {
    public static void main(String[] ars) throws IOException, ParserConfigurationException, SAXException, TransformerException {
        DocumentBuilderFactory df = DocumentBuilderFactory.newInstance();
        DocumentBuilder db = df.newDocumentBuilder();
        System.out.println("<Login><name>root</name><password>root</password></Login>");
        String retMessage = testMeditrash.sendReadMsg(
                "<Login><name>root</name><password>root</password></Login>"
        );
        Document returnMessage = db.parse(new InputSource(new StringReader(retMessage)));
        String token = returnMessage.getElementsByTagName("uniqueToken").item(0).getTextContent();

        XElement rootGetTrashItem = new XElement("Request",
                new XElement("uniqueToken",token),
                new XElement("requestCommand",
                        new XElement("catheory","180101"),
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
        String retGetTrashItem = testMeditrash.sendReadMsg(
                XmlWriter.createDoc(rootGetTrashItem)
        );
        System.out.println();
    }

}
