from io import StringIO
import pickle
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfdocument import PDFDocument
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.pdfpage import PDFPage
from pdfminer.pdfparser import PDFParser
import pprint
import json
import urllib
from urllib import request
import sys
import PyPDF2
import os

# takes in the args from the command line
# exits if there are greater than or less than 3 args, sys.argv[0] is always the program name
if len(sys.argv) != 3:
    print("Usage: <" + sys.argv[0] + "> <PDF url> <output json filename>")
    sys.exit(-1)

# downloads the pdf file at the location
urllib.request.urlretrieve(sys.argv[1], 'input.pdf')
filename = 'input.pdf'
output_filename = sys.argv[2]

# checks for valid PDF file
try:
    a = open(filename, "rb")
    PyPDF2.PdfFileReader(a)
except PyPDF2.utils.PdfReadError:
    # this line, os.remove(filename), might be redundant since we're just going to overwrite it anyway on next
    # program execution
    # os.remove(filename)
    print("Invalid PDF file downloaded")
    sys.exit(-2)
else:
    a.close()
    pass


def extract_text(pdf_file, password='', page_numbers=None, maxpages=0,
                 caching=True, codec='utf-8', laparams=None):
    if laparams is None:
        laparams = LAParams()
    prev = ""
    pages = []
    with open(pdf_file, "rb") as fp:
        output_string = StringIO()
        rsrcmgr = PDFResourceManager()
        device = TextConverter(rsrcmgr, output_string,
                               laparams=laparams)
        interpreter = PDFPageInterpreter(rsrcmgr, device)
        pro = 0
        for page in PDFPage.get_pages(
                fp, page_numbers, maxpages=maxpages,
                password=password, caching=caching,
                check_extractable=True,
        ):
            interpreter.process_page(page)
            current = output_string.getvalue()
            pages.append(current[len(prev):])
            prev = current
            print(pro)
            pro += 1
        print("text extracted")
        return pages

pages = extract_text(filename)

keys = {
    "Department",
    "Program Contact", 
    "Program Offer Type", 
    "Program Offer Stage", 
    "Related Programs",
    "Program Characteristics",
    "Executive Summary", 
    "Program Summary"
}
departments = { 
    "Community Justice", 
    "Community Services", 
    "County Assets", 
    "County Management", 
    "District Attorney",
    "Sheriff", 
    "County Human Services",
    "Health Department", 
    "Library", 
    "Nondepartmental"
}
program_offer_types = {
    "Existing Operating Program", 
    "Support", 
    "Administration",
    "Innovative/New Program", 
    "Internal Service"
}
program_characteristics = {
    "One-Time-Only Request",
    "Backfill State/Federal/Grant", 
    "Measure 5 Education"
}
program_contacts = {}
words = set()

lines = {"Executive Summary", "Program Summary", "Performance Measures Descriptions"}
lines.update(keys)
lines.update(departments)
lines.update(program_offer_types)
lines.update(program_characteristics)
for line in lines:
    word = line.split(' ')
    for w in word:
        words.add(w)

def read_page(page):
    page = page.split("\n")
    data = {k: "" for k in keys}
    data["Program Name"] = page[0]
    data["Executive Summary"] = ""
    data["Program Summary"] = ""

    need_contact = True
    exec_endline = 0

    start_exec = False
    finish_exec = False
    start_prog = False
    finish_prog = False

    for i, line in enumerate(page):
        line = line.encode('ascii', 'ignore')
        line = line.decode('ascii', 'ignore')
        if line == '':
            continue
        elif line[-1] == ':' or i == 0 or line.startswith("www"):
            continue
        if line in departments:
            data["Department"] = line

        elif line in program_offer_types:
            data["Program Offer Type"] = line

        elif line in program_contacts:
            data["Program Contacts"] = line

        elif line.startswith("Program Characteristics:"):
            data["Program Characteristics"] = line.split(": ")[1]

        elif line.startswith("Program Offer Stage: "):
            data["Program Offer Stage"] = line.split(": ")[-1]

        #we're at the start of one of the summaries
        elif len(line) > 40:
            #if we're at the beginning of executive summary
            if not start_exec:
                start_exec = True
            #if we're at the beginning of program summary
            elif finish_exec and not start_prog:
                start_prog = True

        if start_exec and not finish_exec:
            if line == "Program Summary":
                finish_exec = True
            else:
                data["Executive Summary"] += line

        if start_prog and not finish_prog:
            if line == "Performance Measures":
                return data
                finish_prog = True
            else:
                data["Program Summary"] += line

        elif (line.count(' ') == 1 or line.count(' ') == 2) and need_contact:
            potential_name = line.split(' ')
            is_name = True
            for word in potential_name:
                if word in words:
                    is_name = False
            if is_name:
                data["Program Contact"] = line
                need_contact = False

    return data

with open("data.pkl", "rb") as f:
    pages = pickle.load(f)
pages = [p for p in pages if p.startswith("Program #")]
data = []
for p in pages:
    d = read_page(p)
    data.append(d)
with open(output_filename, "w") as f:
    json.dump(data, f, indent="\t")