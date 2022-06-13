using DynStacking.HotStorage.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestSolver {
  public class Program {
    private static World CreateTestWorld() {
      var world = new World();
      world.Now = new TimeStamp() { MilliSeconds = 664000 };

      world.Production = new Stack() { Id = 0, MaxHeight = 4 };
      world.Production.BottomToTop.Add(new Block() { Id = 54, Due = new TimeStamp() { MilliSeconds = 2272000 }, Ready = false });
      world.Production.BottomToTop.Add(new Block() { Id = 53, Due = new TimeStamp() { MilliSeconds = 1603000 }, Ready = false });

      world.Buffers.Add(new Stack() { Id = 1, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 2, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 3, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 4, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 5, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 6, MaxHeight = 8 });

      world.Buffers[0].BottomToTop.Add(new Block() { Id = 36, Due = new TimeStamp() { MilliSeconds = 349000 }, Ready = true });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 3, Due = new TimeStamp() { MilliSeconds = 1000000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 12, Due = new TimeStamp() { MilliSeconds = 220000 }, Ready = true });

      world.Buffers[1].BottomToTop.Add(new Block() { Id = 30, Due = new TimeStamp() { MilliSeconds = -19000 }, Ready = true });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 38, Due = new TimeStamp() { MilliSeconds = 1073000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 42, Due = new TimeStamp() { MilliSeconds = 1072000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 50, Due = new TimeStamp() { MilliSeconds = 1365000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 43, Due = new TimeStamp() { MilliSeconds = 1087000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 52, Due = new TimeStamp() { MilliSeconds = 1387000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 7, Due = new TimeStamp() { MilliSeconds = 1391000 }, Ready = false });

      world.Buffers[2].BottomToTop.Add(new Block() { Id = 23, Due = new TimeStamp() { MilliSeconds = 527000 }, Ready = true });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 16, Due = new TimeStamp() { MilliSeconds = 544000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 1, Due = new TimeStamp() { MilliSeconds = 1041000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 4, Due = new TimeStamp() { MilliSeconds = 1523000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 8, Due = new TimeStamp() { MilliSeconds = 510000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 29, Due = new TimeStamp() { MilliSeconds = 940000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 39, Due = new TimeStamp() { MilliSeconds = 1044000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 5, Due = new TimeStamp() { MilliSeconds = 934000 }, Ready = false });

      world.Buffers[3].BottomToTop.Add(new Block() { Id = 40, Due = new TimeStamp() { MilliSeconds = 970000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 10, Due = new TimeStamp() { MilliSeconds = 518000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 13, Due = new TimeStamp() { MilliSeconds = 438000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 41, Due = new TimeStamp() { MilliSeconds = 1446000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 2, Due = new TimeStamp() { MilliSeconds = 779000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 51, Due = new TimeStamp() { MilliSeconds = 1954000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 14, Due = new TimeStamp() { MilliSeconds = 468000 }, Ready = true });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 37, Due = new TimeStamp() { MilliSeconds = 1106000 }, Ready = false });

      world.Buffers[4].BottomToTop.Add(new Block() { Id = 45, Due = new TimeStamp() { MilliSeconds = 1494000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 44, Due = new TimeStamp() { MilliSeconds = 1537000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 47, Due = new TimeStamp() { MilliSeconds = 1165000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 46, Due = new TimeStamp() { MilliSeconds = 1502000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 9, Due = new TimeStamp() { MilliSeconds = 890000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 17, Due = new TimeStamp() { MilliSeconds = 481000 }, Ready = true });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 49, Due = new TimeStamp() { MilliSeconds = 1489000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 48, Due = new TimeStamp() { MilliSeconds = 2353000 }, Ready = false });

      world.Buffers[5].BottomToTop.Add(new Block() { Id = 32, Due = new TimeStamp() { MilliSeconds = 396000 }, Ready = true });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 31, Due = new TimeStamp() { MilliSeconds = 509000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 6, Due = new TimeStamp() { MilliSeconds = 667000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 15, Due = new TimeStamp() { MilliSeconds = 327000 }, Ready = true });

      world.Handover = new Handover() { Id = 7 };
      world.Handover.Block = new Block() { Id = 11, Due = new TimeStamp() { MilliSeconds = 379000 }, Ready = true };
      world.Handover.Ready = false;

      return world;
    }

    private static World CreateTestWorld2() {
      var world = new World();
      world.Now = new TimeStamp() { MilliSeconds = 664000 };

      world.Production = new Stack() { Id = 0, MaxHeight = 4 };
      world.Production.BottomToTop.Add(new Block() { Id = 63, Due = new TimeStamp() { MilliSeconds = 2009000 }, Ready = false });
      world.Production.BottomToTop.Add(new Block() { Id = 62, Due = new TimeStamp() { MilliSeconds = 2487000 }, Ready = false });
      world.Production.BottomToTop.Add(new Block() { Id = 61, Due = new TimeStamp() { MilliSeconds = 1563000 }, Ready = false });

      world.Buffers.Add(new Stack() { Id = 1, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 2, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 3, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 4, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 5, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 6, MaxHeight = 8 });

      world.Buffers[0].BottomToTop.Add(new Block() { Id = 36, Due = new TimeStamp() { MilliSeconds = 70000 }, Ready = true });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 3, Due = new TimeStamp() { MilliSeconds = 721000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 54, Due = new TimeStamp() { MilliSeconds = 1993000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 55, Due = new TimeStamp() { MilliSeconds = 2037000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 6, Due = new TimeStamp() { MilliSeconds = 388000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 48, Due = new TimeStamp() { MilliSeconds = 2074000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 56, Due = new TimeStamp() { MilliSeconds = 2075000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 1, Due = new TimeStamp() { MilliSeconds = 762000 }, Ready = false });

      world.Buffers[1].BottomToTop.Add(new Block() { Id = 30, Due = new TimeStamp() { MilliSeconds = -298000 }, Ready = true });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 38, Due = new TimeStamp() { MilliSeconds = 794000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 42, Due = new TimeStamp() { MilliSeconds = 793000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 50, Due = new TimeStamp() { MilliSeconds = 1086000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 43, Due = new TimeStamp() { MilliSeconds = 808000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 52, Due = new TimeStamp() { MilliSeconds = 1108000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 60, Due = new TimeStamp() { MilliSeconds = 2424000 }, Ready = false });

      world.Buffers[2].BottomToTop.Add(new Block() { Id = 23, Due = new TimeStamp() { MilliSeconds = 248000 }, Ready = true });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 16, Due = new TimeStamp() { MilliSeconds = 265000 }, Ready = true });

      world.Buffers[3].BottomToTop.Add(new Block() { Id = 40, Due = new TimeStamp() { MilliSeconds = 691000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 10, Due = new TimeStamp() { MilliSeconds = 239000 }, Ready = true });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 13, Due = new TimeStamp() { MilliSeconds = 159000 }, Ready = true });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 41, Due = new TimeStamp() { MilliSeconds = 1167000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 2, Due = new TimeStamp() { MilliSeconds = 500000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 51, Due = new TimeStamp() { MilliSeconds = 1675000 }, Ready = false });

      world.Buffers[4].BottomToTop.Add(new Block() { Id = 45, Due = new TimeStamp() { MilliSeconds = 1215000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 44, Due = new TimeStamp() { MilliSeconds = 1258000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 47, Due = new TimeStamp() { MilliSeconds = 886000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 46, Due = new TimeStamp() { MilliSeconds = 1223000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 9, Due = new TimeStamp() { MilliSeconds = 611000 }, Ready = true });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 53, Due = new TimeStamp() { MilliSeconds = 1324000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 58, Due = new TimeStamp() { MilliSeconds = 1222000 }, Ready = false });
      world.Buffers[4].BottomToTop.Add(new Block() { Id = 7, Due = new TimeStamp() { MilliSeconds = 1112000 }, Ready = false });

      world.Buffers[5].BottomToTop.Add(new Block() { Id = 37, Due = new TimeStamp() { MilliSeconds = 827000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 5, Due = new TimeStamp() { MilliSeconds = 655000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 59, Due = new TimeStamp() { MilliSeconds = 1639000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 49, Due = new TimeStamp() { MilliSeconds = 1210000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 39, Due = new TimeStamp() { MilliSeconds = 765000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 29, Due = new TimeStamp() { MilliSeconds = 661000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 57, Due = new TimeStamp() { MilliSeconds = 1240000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 4, Due = new TimeStamp() { MilliSeconds = 1244000 }, Ready = false });

      world.Handover = new Handover() { Id = 7, Ready = true };

      return world;
    }

    private static World CreateTestWorld3() {
      var world = new World();
      world.Now = new TimeStamp() { MilliSeconds = 267000 };

      world.Production = new Stack() { Id = 0, MaxHeight = 4 };

      world.Buffers.Add(new Stack() { Id = 1, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 2, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 3, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 4, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 5, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 6, MaxHeight = 8 });

      world.Buffers[0].BottomToTop.Add(new Block() { Id = 36, Due = new TimeStamp() { MilliSeconds = 746000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 3, Due = new TimeStamp() { MilliSeconds = 1397000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 19, Due = new TimeStamp() { MilliSeconds = 782000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 21, Due = new TimeStamp() { MilliSeconds = 852000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 37, Due = new TimeStamp() { MilliSeconds = 1503000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 38, Due = new TimeStamp() { MilliSeconds = 1470000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 7, Due = new TimeStamp() { MilliSeconds = 1788000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 26, Due = new TimeStamp() { MilliSeconds = 669000 }, Ready = false });

      world.Buffers[1].BottomToTop.Add(new Block() { Id = 30, Due = new TimeStamp() { MilliSeconds = 377000 }, Ready = true });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 28, Due = new TimeStamp() { MilliSeconds = 258000 }, Ready = true });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 18, Due = new TimeStamp() { MilliSeconds = 526000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 17, Due = new TimeStamp() { MilliSeconds = 878000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 14, Due = new TimeStamp() { MilliSeconds = 865000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 9, Due = new TimeStamp() { MilliSeconds = 1287000 }, Ready = false });

      world.Buffers[2].BottomToTop.Add(new Block() { Id = 23, Due = new TimeStamp() { MilliSeconds = 924000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 16, Due = new TimeStamp() { MilliSeconds = 941000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 1, Due = new TimeStamp() { MilliSeconds = 1438000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 4, Due = new TimeStamp() { MilliSeconds = 1920000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 8, Due = new TimeStamp() { MilliSeconds = 907000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 12, Due = new TimeStamp() { MilliSeconds = 617000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 11, Due = new TimeStamp() { MilliSeconds = 776000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 29, Due = new TimeStamp() { MilliSeconds = 1337000 }, Ready = false });

      world.Buffers[3].BottomToTop.Add(new Block() { Id = 39, Due = new TimeStamp() { MilliSeconds = 1441000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 22, Due = new TimeStamp() { MilliSeconds = 637000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 40, Due = new TimeStamp() { MilliSeconds = 1367000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 13, Due = new TimeStamp() { MilliSeconds = 835000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 10, Due = new TimeStamp() { MilliSeconds = 915000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 41, Due = new TimeStamp() { MilliSeconds = 1843000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 6, Due = new TimeStamp() { MilliSeconds = 1064000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 42, Due = new TimeStamp() { MilliSeconds = 1469000 }, Ready = false });

      world.Buffers[4].BottomToTop.Add(new Block() { Id = 34, Due = new TimeStamp() { MilliSeconds = 198000 }, Ready = true });

      world.Buffers[5].BottomToTop.Add(new Block() { Id = 32, Due = new TimeStamp() { MilliSeconds = 793000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 31, Due = new TimeStamp() { MilliSeconds = 906000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 15, Due = new TimeStamp() { MilliSeconds = 724000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 20, Due = new TimeStamp() { MilliSeconds = 673000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 24, Due = new TimeStamp() { MilliSeconds = 394000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 43, Due = new TimeStamp() { MilliSeconds = 1484000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 5, Due = new TimeStamp() { MilliSeconds = 1331000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 2, Due = new TimeStamp() { MilliSeconds = 1176000 }, Ready = false });

      world.Handover = new Handover() { Id = 7, Ready = true };

      return world;
    }

    private static World CreateTestWorld4() {
      var world = new World();
      world.Now = new TimeStamp() { MilliSeconds = 267000 };

      world.Production = new Stack() { Id = 0, MaxHeight = 4 };

      world.Buffers.Add(new Stack() { Id = 1, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 2, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 3, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 4, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 5, MaxHeight = 8 });
      world.Buffers.Add(new Stack() { Id = 6, MaxHeight = 8 });

      world.Buffers[0].BottomToTop.Add(new Block() { Id = 36, Due = new TimeStamp() { MilliSeconds = 746000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 3, Due = new TimeStamp() { MilliSeconds = 1397000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 19, Due = new TimeStamp() { MilliSeconds = 782000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 21, Due = new TimeStamp() { MilliSeconds = 852000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 37, Due = new TimeStamp() { MilliSeconds = 1503000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 38, Due = new TimeStamp() { MilliSeconds = 1470000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 7, Due = new TimeStamp() { MilliSeconds = 1788000 }, Ready = false });
      world.Buffers[0].BottomToTop.Add(new Block() { Id = 26, Due = new TimeStamp() { MilliSeconds = 669000 }, Ready = false });

      world.Buffers[1].BottomToTop.Add(new Block() { Id = 30, Due = new TimeStamp() { MilliSeconds = 377000 }, Ready = true });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 28, Due = new TimeStamp() { MilliSeconds = 258000 }, Ready = true });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 18, Due = new TimeStamp() { MilliSeconds = 526000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 17, Due = new TimeStamp() { MilliSeconds = 878000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 14, Due = new TimeStamp() { MilliSeconds = 865000 }, Ready = false });
      world.Buffers[1].BottomToTop.Add(new Block() { Id = 9, Due = new TimeStamp() { MilliSeconds = 1287000 }, Ready = false });

      world.Buffers[2].BottomToTop.Add(new Block() { Id = 23, Due = new TimeStamp() { MilliSeconds = 924000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 16, Due = new TimeStamp() { MilliSeconds = 941000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 1, Due = new TimeStamp() { MilliSeconds = 1438000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 4, Due = new TimeStamp() { MilliSeconds = 1920000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 8, Due = new TimeStamp() { MilliSeconds = 907000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 12, Due = new TimeStamp() { MilliSeconds = 617000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 11, Due = new TimeStamp() { MilliSeconds = 776000 }, Ready = false });
      world.Buffers[2].BottomToTop.Add(new Block() { Id = 29, Due = new TimeStamp() { MilliSeconds = 1337000 }, Ready = false });

      world.Buffers[3].BottomToTop.Add(new Block() { Id = 39, Due = new TimeStamp() { MilliSeconds = 1441000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 22, Due = new TimeStamp() { MilliSeconds = 637000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 40, Due = new TimeStamp() { MilliSeconds = 1367000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 13, Due = new TimeStamp() { MilliSeconds = 835000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 10, Due = new TimeStamp() { MilliSeconds = 915000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 41, Due = new TimeStamp() { MilliSeconds = 1843000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 6, Due = new TimeStamp() { MilliSeconds = 1064000 }, Ready = false });
      world.Buffers[3].BottomToTop.Add(new Block() { Id = 42, Due = new TimeStamp() { MilliSeconds = 1469000 }, Ready = false });

      // empty buffer 4

      world.Buffers[5].BottomToTop.Add(new Block() { Id = 32, Due = new TimeStamp() { MilliSeconds = 793000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 31, Due = new TimeStamp() { MilliSeconds = 906000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 15, Due = new TimeStamp() { MilliSeconds = 724000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 20, Due = new TimeStamp() { MilliSeconds = 673000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 24, Due = new TimeStamp() { MilliSeconds = 394000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 43, Due = new TimeStamp() { MilliSeconds = 1484000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 5, Due = new TimeStamp() { MilliSeconds = 1331000 }, Ready = false });
      world.Buffers[5].BottomToTop.Add(new Block() { Id = 2, Due = new TimeStamp() { MilliSeconds = 1176000 }, Ready = false });

      world.Handover = new Handover() { Id = 7, Ready = true };

      return world;
    }

    static void Main(string[] args) {
      var state = new csharp.HS_Sync.RFState(CreateTestWorld3());

      var x = state.GetBestMovesBeam(6, 5);
      var y = x.Item1.ConsolidateMoves();
    }
  }

  public static class Extensions {
    public static IEnumerable<CraneMove> ConsolidateMoves(this List<CraneMove> moves) {
      List<CraneMove> cleanList = new List<CraneMove>();

      for (int i = 0; i < moves.Count() - 1; i++) {
        var move = moves[i];
        var nextMove = moves[i + 1];

        if (move.BlockId == nextMove.BlockId) {
          if (move.SourceId == nextMove.TargetId) {
            i++;
            continue;
          } else if (move.TargetId == nextMove.SourceId) {
            i++;
            cleanList.Add(new CraneMove() { BlockId = move.BlockId, SourceId = move.SourceId, TargetId = nextMove.TargetId });
            // moves are useless
          }
        } else {
          cleanList.Add(move);
        }
      }

      return cleanList;
    }
  }
}
