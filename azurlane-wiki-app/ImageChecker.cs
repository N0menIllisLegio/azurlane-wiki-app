using System;
using System.IO;


namespace azurlane_wiki_app
{
    class ImageChecker
    {
        private string _sFilename;
        private Types _eFileType = Types.FileNotFound;
        private bool _blComplete = false;
        private int _iEndingNull = 0;

        private readonly byte[] _abTagPNG = {137, 80, 78, 71, 13, 10, 26, 10};
        private readonly byte[] _abTagJPG = {255, 216, 255};
        private readonly byte[] _abTagGIFa = {71, 73, 70, 56, 55, 97};
        private readonly byte[] _abTagGIFb = {71, 73, 70, 56, 57, 97};
        private readonly byte[] _abEndPNG = {73, 69, 78, 68, 174, 66, 96, 130};
        private readonly byte[] _abEndJPGa = {255, 217, 255, 255};
        private readonly byte[] _abEndJPGb = {255, 217};
        private readonly byte[] _abEndGIF = {0, 59};

        public enum Types
        {
            FileNotFound,
            FileEmpty,
            FileNull,
            FileTooLarge,
            FileUnrecognized,
            PNG,
            JPG,
            GIFa,
            GIFb
        }

        public ImageChecker(string filename, bool cullEndingNullBytes = true, int maxFileSize = int.MaxValue)
        {
            _sFilename = filename.Trim();
            FileInfo fliTmp = (new FileInfo(_sFilename));
            // check file exists...
            if (File.Exists(_sFilename))
            {
                _eFileType = Types.FileUnrecognized; // default if found
                // check file has data...
                if (fliTmp.Length == 0)
                {
                    _eFileType = Types.FileEmpty;
                }
                else
                {
                    // check file isn't like stupid crazy big
                    if (fliTmp.Length > maxFileSize)
                    {
                        _eFileType = Types.FileTooLarge;
                    }
                    else
                    {
                        // load the whole file
                        byte[] abtTmp = File.ReadAllBytes(_sFilename);
                        // check the length of actual data
                        int iLength = abtTmp.Length;
                        if (abtTmp[abtTmp.Length - 1] == 0)
                        {
                            for (int i = (abtTmp.Length - 1); i > -1; i--)
                            {
                                if (abtTmp[i] != 0)
                                {
                                    iLength = i;
                                    break;
                                }
                            }
                        }

                        // check that there is actual data
                        if (iLength == 0)
                        {
                            _eFileType = Types.FileNull;
                        }
                        else
                        {
                            _iEndingNull = (abtTmp.Length - iLength);
                            // resize the data so we can work with it
                            Array.Resize<byte>(ref abtTmp, iLength);
                            // get the file type
                            if (_StartsWith(abtTmp, _abTagPNG))
                            {
                                _eFileType = Types.PNG;
                            }
                            else if (_StartsWith(abtTmp, _abTagJPG))
                            {
                                _eFileType = Types.JPG;
                            }
                            else if (_StartsWith(abtTmp, _abTagGIFa))
                            {
                                _eFileType = Types.GIFa;
                            }
                            else if (_StartsWith(abtTmp, _abTagGIFb))
                            {
                                _eFileType = Types.GIFb;
                            }

                            // check the file is complete
                            switch (_eFileType)
                            {
                                case Types.PNG:
                                    _blComplete = _EndsWidth(abtTmp, _abEndPNG);
                                    break;
                                case Types.JPG:
                                    _blComplete = (_EndsWidth(abtTmp, _abEndJPGa) || _EndsWidth(abtTmp, _abEndJPGb));
                                    break;
                                case Types.GIFa:
                                case Types.GIFb:
                                    _blComplete = _EndsWidth(abtTmp, _abEndGIF);
                                    break;
                            }

                            // get rid of ending null bytes at caller's option
                           // if (_blComplete && cullEndingNullBytes) File.WriteAllBytes(_sFilename, abtTmp);
                        }
                    }
                }
            }
        }

        public string Filename
        {
            get { return _sFilename; }
        }

        public Types FileType
        {
            get { return _eFileType; }
        }

        public bool IsComplete
        {
            get { return _blComplete; }
        }

        public int EndingNullBytes
        {
            get { return _iEndingNull; }
        }

        private bool _StartsWith(byte[] data, byte[] search)
        {
            bool blRet = false;
            if (search.Length <= data.Length)
            {
                blRet = true;
                for (int i = 0; i < search.Length; i++)
                {
                    if (data[i] != search[i])
                    {
                        blRet = false;
                        break;
                    }
                }
            }

            return blRet; // RETURN
        }

        private bool _EndsWidth(byte[] data, byte[] search)
        {
            bool blRet = false;
            if (search.Length <= data.Length)
            {
                int iStart = (data.Length - search.Length);
                blRet = true;
                for (int i = 0; i < search.Length; i++)
                {
                    if (data[iStart + i] != search[i])
                    {
                        blRet = false;
                        break;
                    }
                }
            }

            return blRet; // RETURN
        }
    }
}