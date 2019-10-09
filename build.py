import sys
import shutil
import os
import subprocess

dependencies = ["Budget.cs",
                "Campaign.cs",
                "Character.cs",
                "Coordinate.cs", 
                "Database.cs",
                "Date.cs",
                "Faction.cs",
                "Government.cs",
                "Planet.cs",
                "Player.cs",
                "Serializer.cs",
                "ShipModel.cs",
                "Weapon.cs"]

def test():
    build()
    capture = subprocess.run(["/mnt/c/Windows/Microsoft.NET/Framework/v4.0.30319/csc.exe", "-target:exe", "-out:DatabaseTest.exe", "-reference:GSWS.Database.dll", "-reference:JMSuite.Testing.dll", "DatabaseTest.cs"], capture_output=True, text=True)
    output = capture.stdout.split() + capture.stderr.split()
    # print(output)
    if 'error' in output:
        print('ERROR while compiling tests:')
        print(capture.stdout)
        print(capture.stderr)
        print('Quitting')
        exit(1)
    elif 'warning' in output:
        print('WARNING while compiling tests')
        print(capture.stdout)
        print(capture.stderr)
        print('Continuing')
    capture = subprocess.run(["./DatabaseTest.exe"], capture_output=True, text=True)
    output = capture.stdout.split() + capture.stderr.split()
    # print(output)
    if 'failed.' in output or 'Exception:' in output:
        print('Some tests failed:')
        print(capture.stdout)
        print(capture.stderr)
    else:
        print('All tests passed!')
    subprocess.run(["rm", "DatabaseTest.exe"])

def build():
    subprocess.run(["rm", "GSWS.Database.dll"])
    capture = subprocess.run(["/mnt/c/Windows/Microsoft.NET/Framework/v4.0.30319/csc.exe", "-target:library", "-reference:JMSuite.Collections.Graph.dll", "-out:GSWS.Database.dll"] + dependencies, capture_output=True, text=True)
    output = capture.stdout.split() + capture.stderr.split()
    if 'error' in output:
        print('ERROR while compiling library:')
        print(capture.stdout)
        print(capture.stderr)
        print('Quitting')
        exit(1)
    elif 'warning' in output:
        print('WARNING while compiling library:')
        print(capture.stdout)
        print(capture.stderr)
        print('Continuing')
    print('Library built!')

def change_directory(directory):
    os.chdir(directory)

def compile(filename):
    subprocess.run(["/mnt/c/Windows/Microsoft.NET/Framework/v4.0.30319/csc.exe", "-target:library", "-out:jmsuite-" + filename + ".dll", filename + ".cs"])

if __name__ == "__main__":
    subprocess.run(['clear'])
    if len(sys.argv) < 2:
        print("Supply one of the following arguments: 'build' | 'test'")
    elif sys.argv[1] == "build" or sys.argv[1] == "b":
        build()
    elif sys.argv[1] == "test" or sys.argv[1] == "t":
        test()
    else:
        print("Supply one of the following arguments: 'build' | 'test'")