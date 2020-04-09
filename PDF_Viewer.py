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
import unicodedata

filename = "b1.pdf"
output_filename = "textified.txt"
textoutput = "output.txt"

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
                fp,
                page_numbers,
                maxpages=maxpages,
                password=password,
                caching=caching,
                check_extractable=True,
        ):
            interpreter.process_page(page)
            current = output_string.getvalue()
            pages.append(current[len(prev):])
            prev = current
            print(pro)
            pro += 1
            
        return pages
        
pages = extract_text(filename)

with open("data.pkl", "wb") as f:
    pickle.dump(pages, f)
print()

def start_with_field(t, f):
    for f1 in f:
        if t.startswith(f1):
            return f1
            
def read_page(s, f):
    s = s.split("\n")
    
    for i, l in enumerate(s):
        l = l.encode('ascii', 'ignore')
        l = l.decode('ascii', 'ignore')
        f.write(str(i) + "\t" + l + '\n')
        if l == "Performance Measures":
            break
        #if i > 20: 
           # break
    f.write('\n\n')

with open("data.pkl", "rb") as f:
    pages = pickle.load(f)

f = open(output_filename, "w")
pages = [p for p in pages if p.startswith("Program #")]
for p in pages:
    read_page(p, f)
f.close()
    