import subprocess
from argparse import ArgumentParser

parser = ArgumentParser()
parser.add_argument("-f", "--files", nargs="+", dest="files", help="Define a .csproj file list.")

args = parser.parse_args()

for f in args.files:
	subprocess.call(r"C:\Program Files\dotnet\dotnet.exe" +  " test " + f)