import sys
import csv
import xml.etree.ElementTree as ET

ty = ''
output = ''
vals = { 'Body': ['Coordinates', 'kSystem', 'kSector', 'Region', 'Class', 
                  'Atmosphere', 'Gravity', 'Terrain', 'Demonym', 'DayLength', 
                  'YearLength', 'Climate', 'Surface', 'Diameter'],
         'Planet': ['Name', 'Population', 'Wealth', 'kGovernment']}

def main():
    input()
    if ty == 'bodies' or ty == 'b':
        csv_to_xml('Body')
    elif ty == 'planets' or ty == 'p':
        csv_to_xml('Planet')

def input():
    global ty, output
    ty = sys.argv[1]
    output = sys.argv[2]

def csv_to_xml(cls):
    c = csv.DictReader(sys.stdin)
    dic = ET.Element('JDictionaryOfString' + cls)
    for r in c:
        item = ET.SubElement(dic, 'item')
        key = ET.SubElement(item, 'key')
        ET.SubElement(key, 'string').text = r['ID']
        value = ET.SubElement(item, 'value')
        body = ET.SubElement(value, cls)
        body.set('ID', r['ID'])
        fields = vals[cls]
        for f in fields:
            if f == 'Coordinates':
                coords = ET.SubElement(body, 'Coordinates')
                ET.SubElement(coords, 'X').text = r['X']
                ET.SubElement(coords, 'Y').text = r['Y']
                ET.SubElement(coords, 'Z').text = r['Z']
            elif f[0] == 'k':
                ET.SubElement(body, f).text = r[f[1:len(f)]]
            else:
                ET.SubElement(body, f).text = r[f]
    tree = ET.ElementTree()
    tree._setroot(dic)
    tree.write(output, encoding='utf-8', xml_declaration=True)

def bodies_to_xml():
    c = csv.DictReader(sys.stdin)
    dic = ET.Element('JDictionaryOfStringBody')
    for r in c:
        item = ET.SubElement(dic, 'item')
        key = ET.SubElement(item, 'key')
        ET.SubElement(key, 'string').text = r['ID']
        value = ET.SubElement(item, 'value')
        body = ET.SubElement(value, 'Body')
        body.set('ID', r['ID'])
        ET.SubElement(body, 'Name').text = r['Name']
        coords = ET.SubElement(body, 'Coordinates')
        ET.SubElement(coords, 'X').text = r['X']
        ET.SubElement(coords, 'Y').text = r['Y']
        ET.SubElement(coords, 'Z').text = r['Z']
        fields = ['System', 'Sector', 'Region', 'Class', 'Atmosphere', 
                  'Gravity', 'Terrain', 'Demonym', 'DayLength', 'YearLength', 
                  'Climate', 'Surface', 'Diameter']
        for f in fields:
            ET.SubElement(body, f).text = r[f]
    tree = ET.ElementTree()
    tree._setroot(dic)
    tree.write(output, encoding='utf-8', xml_declaration=True)

def planets_to_xml():
    c = csv.DictReader(sys.stdin)
    dic = ET.Element('JDictionaryOfStringPlanet')
    for r in c:
        item = ET.SubElement(dic, 'item')
        key = ET.SubElement(item, 'key')
        ET.SubElement(key, 'string').text = r['ID']
        value = ET.SubElement(item, 'value')
        body = ET.SubElement(value, 'Planet')
        body.set('ID', r['ID'])
        fields = ['Population', 'Wealth', 'Government']
        for f in fields:
            ET.SubElement(body, f).text = r[f]
    tree = ET.ElementTree()
    tree._setroot(dic)
    tree.write(output, encoding='utf-8', xml_declaration=True)

if __name__ == "__main__":
    main()