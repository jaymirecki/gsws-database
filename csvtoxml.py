import sys
import csv
import xml.etree.ElementTree as ET

ty = ''
output = ''
vals = { 'Body':       ['Position', 'System', 'Sector', 'Region', 'Class', 
                        'Atmosphere', 'Gravity', 'Terrain',
                        'DayLength', 'YearLength', 'Climate', 'Surface', 
                        'Diameter'],
         'Planet':     ['Name', 'Population', 'Wealth', 'Demonym', 
                        'Description', 'kGovernment'],
         'Fleet':      ['Name', 'kDestination', 'kNextStop', 'kOrbiting', 
                        'Position', 'kMilitary'],
         'Government': ['Name', 'Color', 'kSuperGovernment', 'kMilitary', 
                        'ExecutivePower', 'LegislativePower', 'JudicialPower', 
                        'ResidentialTax', 'IndustrialTax', 'Budget', 
                        'Description', 'Relationships'],
         'Military':   ['Name', 'kSuperMilitary', 'kGovernment', 'Description'] }

def main():
    global directory
    input()
    if ty == 'all' or ty == 'a':
        choose_serialization('bodies')
        choose_serialization('planets')
        choose_serialization('fleets')
        choose_serialization('governments')
        choose_serialization('militaries')
    else:
        choose_serialization(ty)

def input():
    global ty, directory
    ty = sys.argv[1]
    directory = sys.argv[2]

def choose_serialization(command):
    global file
    if command == 'bodies' or command == 'b':
        file = directory + 'bodies'
        csv_to_xml('Body')
    elif command == 'planets' or command == 'p':
        file = directory + 'planets'
        csv_to_xml('Planet')
    elif command == 'fleets' or command == 'f':
        file = directory + 'fleets'
        csv_to_xml('Fleet')
    elif command == 'governments' or command == 'g':
        file = directory + 'governments'
        csv_to_xml('Government')
    elif command == 'militaries' or command == 'm':
        file = directory + 'militaries'
        csv_to_xml('Military')

def csv_to_xml(cls):
    f = open(file + '.csv')
    c = csv.DictReader(f)
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
            if f == 'Position':
                if r['X'] != '':
                    coords = ET.SubElement(body, 'Position')
                    ET.SubElement(coords, 'X').text = r['X']
                    ET.SubElement(coords, 'Y').text = r['Y']
                    ET.SubElement(coords, 'Z').text = r['Z']
            elif f == 'Budget':
                budget = ET.SubElement(body, 'Budget')
                if r['Military'] != '':
                    ET.SubElement(budget, 'Military').text = r['Military']
                if r['PublicSafety'] != '':
                    ET.SubElement(budget, 'PublicSafety').text = r['PublicSafety']
                if r['Health'] != '':
                    ET.SubElement(budget, 'Health').text = r['Health']
                if r['Education'] != '':
                    ET.SubElement(budget, 'Education').text = r['Education']
                if r['Balance'] != '':
                    ET.SubElement(budget, 'Balance').text = r['Balance']
            elif f == 'Relationships':
                reldic = ET.SubElement(body, 'Relationships')
                for i in range(1, 10):
                    try:
                        r['RelationshipGroup' + str(i)]
                    except:
                        continue
                    else:
                        relitem = ET.SubElement(reldic, 'item')
                        relkey = ET.SubElement(relitem, 'key')
                        ET.SubElement(relkey, 'string').text = r['RelationshipGroup' + str(i)]
                        relvalue = ET.SubElement(relitem, 'value')
                        ET.SubElement(relvalue, 'Relationship').text = r['RelationshipStatus' + str(i)]
            elif r[f] != '':
                ET.SubElement(body, f).text = r[f]
    tree = ET.ElementTree()
    tree._setroot(dic)
    # close(f)
    tree.write(file + '.xml', encoding='utf-8', xml_declaration=True)

if __name__ == "__main__":
    main()