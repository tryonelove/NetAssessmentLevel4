namespace NetAssessmentLevel4;

public class ReaderWriterLockMicrosoftTest
{
   private const int NumThreads = 26;

   private static readonly ReaderWriterLock Rwl = new();

   private static int _resource;
   private static int _readerTimeouts;
   private static int _writerTimeouts;
   private static int _reads;
   private static int _writes;
   private static bool _running = true;


   public static void Start()
   {
      var t = new Thread[NumThreads];
      for (var i = 0; i < NumThreads; i++){
         t[i] = new Thread(ThreadProc)
         {
            Name = new string((char)(i + 65), 1)
         };

         t[i].Start();
         if (i > 10)
         {
            Thread.Sleep(300);
         }
      }

      _running = false;
      for (var i = 0; i < NumThreads; i++)
      {
         t[i].Join();
      }

      Console.WriteLine("\n{0} reads, {1} writes, {2} reader time-outs, {3} writer time-outs.",
            _reads, _writes, _readerTimeouts, _writerTimeouts);
      Console.Write("Press ENTER to exit... ");
      Console.ReadLine();
   }

   private static void ThreadProc()
   {
      var rnd = new Random();

      while (_running)
      {
         var action = rnd.NextDouble();
         switch (action)
         {
            case < .8:
               ReadFromResource(10);
               break;
            case < .81:
               ReleaseRestore(rnd, 50);
               break;
            case < .90:
               UpgradeDowngrade(rnd, 100);
               break;
            default:
               WriteToResource(rnd, 100);
               break;
         }
      }
   }

   private static void ReadFromResource(int timeOut)
   {
      try {
         Rwl.AcquireReaderLock(timeOut);
         try {
            Display("reads resource value " + _resource);
            Interlocked.Increment(ref _reads);
         }
         finally {
            Rwl.ReleaseReaderLock();
         }
      }
      catch (ApplicationException) {
         Interlocked.Increment(ref _readerTimeouts);
      }
   }

   private static void WriteToResource(Random rnd, int timeOut)
   {
      try {
         Rwl.AcquireWriterLock(timeOut);
         try {
            _resource = rnd.Next(500);
            Display("writes resource value " + _resource);
            Interlocked.Increment(ref _writes);
         }
         finally {
            Rwl.ReleaseWriterLock();
         }
      }
      catch (ApplicationException) {
         Interlocked.Increment(ref _writerTimeouts);
      }
   }

   private static void UpgradeDowngrade(Random rnd, int timeOut)
   {
      try {
         Rwl.AcquireReaderLock(timeOut);
         try {
            Display("reads resource value " + _resource);
            Interlocked.Increment(ref _reads);

            try {
               LockCookie lc = Rwl.UpgradeToWriterLock(timeOut);
               try {
                  _resource = rnd.Next(500);
                  Display("writes resource value " + _resource);
                  Interlocked.Increment(ref _writes);
               }
               finally {
                  Rwl.DowngradeFromWriterLock(ref lc);
               }
            }
            catch (ApplicationException) {
               Interlocked.Increment(ref _writerTimeouts);
            }

            Display("reads resource value " + _resource);
            Interlocked.Increment(ref _reads);
         }
         finally {
            Rwl.ReleaseReaderLock();
         }
      }
      catch (ApplicationException) {
         Interlocked.Increment(ref _readerTimeouts);
      }
   }

   private static void ReleaseRestore(Random rnd, int timeOut)
   {
      try {
         Rwl.AcquireReaderLock(timeOut);
         try {
            // It's safe for this thread to read from the shared resource,
            // so read and cache the resource value.
            int resourceValue = _resource;     // Cache the resource value.
            Display("reads resource value " + resourceValue);
            Interlocked.Increment(ref _reads);

            // Save the current writer sequence number.
            var lastWriter = Rwl.WriterSeqNum;

            // Release the lock and save a cookie so the lock can be restored later.
            var lc = Rwl.ReleaseLock();

            // Wait for a random interval and then restore the previous state of the lock.
            Thread.Sleep(rnd.Next(250));
            Rwl.RestoreLock(ref lc);

            // Check whether other threads obtained the writer lock in the interval.
            // If not, then the cached value of the resource is still valid.
            if (Rwl.AnyWritersSince(lastWriter)) {
               resourceValue = _resource;
               Interlocked.Increment(ref _reads);
               Display("resource has changed " + resourceValue);
            }
            else {
               Display("resource has not changed " + resourceValue);
            }
         }
         finally {
            Rwl.ReleaseReaderLock();
         }
      }
      catch (ApplicationException) {
         Interlocked.Increment(ref _readerTimeouts);
      }
   }

   private static void Display(string msg)
   {
      Console.Write("Thread {0} {1}.       \r", Thread.CurrentThread.Name, msg);
   }
}