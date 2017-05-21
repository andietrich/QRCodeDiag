# QRCodeDiag
Diagnosis tool for damaged QRCodes where ECC alone doesn't help.

Normal decoders just fail to read the code if there are are too many bits missing.
This tool is supposed to help in reading the partial information that is left.
Often additional information is known or can be guessed, e.g.the character encoding or parts of the information.
The additional knowledge can be used to restore parts of the code, so that maybe the threshold for error correction is reached or the required information can be extracted.
