package testMeditrash;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class XElement {
    String name;
    List<XElement> children;
    List<MutPair<String,String>> attributes;
    String value;
    public XElement(String name, XElement... args){
        this.name = name;
        children = new ArrayList<>(Arrays.asList(args));
        attributes = new ArrayList<>();
        this.value = value;
    }
    public XElement(String name, String value){
        this.name = name;
        children = new ArrayList<>();
        attributes = new ArrayList<>();
        this.value = value;
    }
    public XElement(String name,String value, XElement... args){
        this.name = name;
        children = new ArrayList<>(Arrays.asList(args));
        attributes = new ArrayList<>();
        this.value = value;
    }

    public void setAttribute(String name, String value) {
        MutPair<String,String> attribute = null;
        for (MutPair<String,String> attributePair : attributes) {
            if(attributePair.getFirst().equals(name)){
                attribute=attributePair;
            }
        }
        if(attribute==null){
            attributes.add(new MutPair<>(name,value));
        }
        else{
            attribute.setSecond(value);
        }
    }
}
