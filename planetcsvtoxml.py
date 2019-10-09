import csv
file = open("planets.csv", 'r', newline='')
csvrdr = csv.reader(file)
next(csvrdr)
output = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<ArrayOfPlanet xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\n"
for row in csvrdr:
    print(row[0])
    output = output + "  <Planet ID=\"" + row[0] + "\">\n"
    output = output + "    <Name>" + row[1] + "</Name>\n"
    output = output + "    <System>" + row[5] + "</System>\n"
    output = output + "    <Sector>" + row[6] + "</Sector>\n"
    output = output + "    <Region>" + row[7] + "</Region>\n"
    output = output + "    <Class>" + row[10] + "</Class>\n"
    output = output + "    <Climate>" + row[12] + "</Climate>\n"
    output = output + "    <Demonym>" + row[16] + "</Demonym>\n"
    output = output + "    <Faction>" + row[18] + "</Faction>\n"
    output = output + "    <Economy>" + row[19] + "</Economy>\n"
    output = output + "    <Coordinates>\n      <X>" + row[2] + "</X>\n"
    output = output + "      <Y>" + row[3] + "</Y>\n"
    output = output + "      <Z>" + row[4] + "</Z>\n    </Coordinates>\n"
    output = output + "    <DayLength>" + row[8] + "</DayLength>\n"
    output = output + "    <YearLength>" + row[9] + "</YearLength>\n"
    output = output + "    <AtmosphereType>" + row[11] + "</AtmosphereType>\n"
    output = output + "    <Diameter>" + row[14] + "</Diameter>\n"
    output = output + "    <Gravity>" + row[13] + "</Gravity>\n"
    output = output + "    <AvailableSurface>" + row[15] + "</AvailableSurface>\n"
    output = output + "    <PopulationEconomicPosition>" + row[20] + "</PopulationEconomicPosition>\n"
    output = output + "    <PopulationSocialPosition>" + row[21] + "</PopulationSocialPosition>\n"
    output = output + "    <Population>" + row[17] + "</Population>\n"
    output = output + "    <Wealth>" + row[22] + "</Wealth>\n"
    output = output + "    <Industrialization>" + row[23] + "</Industrialization>\n"
    output = output + "    <Productivity>" + row[24] + "</Productivity>\n"
    output = output + "    <PopulationCapacity>" + row[25] + "</PopulationCapacity>\n"
    output = output + "    <IndustrialCapacity>" + row[26] + "</IndustrialCapacity>\n"
    output = output + "    <UnusedCapacity>" + row[27] + "</UnusedCapacity>\n"
    output = output + "    <MaxCapacity>" + row[28] + "</MaxCapacity>\n"
    output = output + "    <Neighbors>\n"
    for i in range(29, len(row) - 1):
        if len(row[i]) > 0:
            output = output + "      <string>" + row[i] + "</string>\n"
    output = output + "    </Neighbors>\n"
    output = output + "  </Planet>\n"
output = output + "</ArrayOfPlanet>"
outputfile = open("planets.xml", 'w')
outputfile.write(output)