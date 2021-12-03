using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    public class TransactionLogger {
        //Vi vil ikke opbevare fil referencen sådan at man kan redigere filen undervejs osv.
        string _targetFile;

        public TransactionLogger(string targetFile) {
            _targetFile = targetFile;
        }

        public void LogTransaction(Transaction transaction) {
            //Hvis vi ikke kan åbne filen så kan vi ikke logge transaktionen og derfor kan vi
            //ikke gennemfører en transaktion, så vi lader bare exepcetion bobble op til toppen
            using (StreamWriter file = File.AppendText(_targetFile)) {
                file.Write($"{transaction}\n");
            }
        }
    }
}
