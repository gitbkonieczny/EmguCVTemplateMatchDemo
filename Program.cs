using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TemplateMatchDemo
{
    class Program
    {
        public static List<BoardSocket> Sockets = new List<BoardSocket>();
        static void Main(string[] args)
        {
            //Creating base Socket configuration
            Sockets.Add(new BoardSocket(new Point(500, 180), new Size(130,137),"30A")); //Large 30A left-up
            Sockets.Add(new BoardSocket(new Point(611, 226), new Size(69, 48),"10A"));
            Sockets.Add(new BoardSocket(new Point(709, 172), new Size(153, 140),"40A"));
            Sockets.Add(new BoardSocket(new Point(941, 119), new Size(138, 138),"50A"));
            Sockets.Add(new BoardSocket(new Point(825, 229), new Size(113, 79), "None"));
            Sockets.Add(new BoardSocket(new Point(915, 234), new Size(142, 90), "None"));
            Sockets.Add(new BoardSocket(new Point(822, 280), new Size(119, 94), "None"));
            Sockets.Add(new BoardSocket(new Point(670, 271), new Size(176, 100), "None"));
            Sockets.Add(new BoardSocket(new Point(574, 273), new Size(127, 65), "7.5A"));
            Sockets.Add(new BoardSocket(new Point(914, 289), new Size(134, 69), "10A"));
            Sockets.Add(new BoardSocket(new Point(426, 317), new Size(169, 66), "15A"));
            Sockets.Add(new BoardSocket(new Point(575, 320), new Size(134, 71), "15A"));
            Sockets.Add(new BoardSocket(new Point(685, 333), new Size(160, 87), "30A"));
            Sockets.Add(new BoardSocket(new Point(825, 337), new Size(113, 71), "10A"));
            Sockets.Add(new BoardSocket(new Point(991, 343), new Size(115, 74), "None"));
            Sockets.Add(new BoardSocket(new Point(585, 379), new Size(110, 69), "10A"));
            Sockets.Add(new BoardSocket(new Point(675, 388), new Size(180, 75), "30A"));
            Sockets.Add(new BoardSocket(new Point(828, 395), new Size(109, 74), "10A"));
            Sockets.Add(new BoardSocket(new Point(918, 392), new Size(119, 83), "None"));
            Sockets.Add(new BoardSocket(new Point(578, 439), new Size(115, 63), "15A"));
            Sockets.Add(new BoardSocket(new Point(680, 450), new Size(169, 62), "30A"));
            Sockets.Add(new BoardSocket(new Point(828, 450), new Size(108, 63), "7.5A"));
            Sockets.Add(new BoardSocket(new Point(919, 464), new Size(114, 79), "10A"));
            Sockets.Add(new BoardSocket(new Point(427, 483), new Size(169, 66), "30A"));
            Sockets.Add(new BoardSocket(new Point(581, 479), new Size(102, 69), "7.5A"));
            Sockets.Add(new BoardSocket(new Point(678, 500), new Size(163, 61), "25A"));
            Sockets.Add(new BoardSocket(new Point(827, 494), new Size(101, 79), "None"));
            Sockets.Add(new BoardSocket(new Point(912, 495), new Size(119, 85), "None"));
            Sockets.Add(new BoardSocket(new Point(434, 540), new Size(166, 66), "30A"));
            Sockets.Add(new BoardSocket(new Point(583, 545), new Size(167, 70), "None"));
            Sockets.Add(new BoardSocket(new Point(725, 542), new Size(115, 78), "None"));
            Sockets.Add(new BoardSocket(new Point(820, 548), new Size(111, 74), "10A"));
            Sockets.Add(new BoardSocket(new Point(920, 554), new Size(121, 70), "10A"));
            Sockets.Add(new BoardSocket(new Point(435, 590), new Size(175, 74), "10A"));
            Sockets.Add(new BoardSocket(new Point(595, 591), new Size(158, 69), "30A"));
            Sockets.Add(new BoardSocket(new Point(769, 607), new Size(115, 61), "None"));
            Sockets.Add(new BoardSocket(new Point(892, 607), new Size(148, 69), "15A"));
            Sockets.Add(new BoardSocket(new Point(433, 642), new Size(58, 138), "10A"));
            Sockets.Add(new BoardSocket(new Point(495, 697), new Size(144, 140), "40A"));
            Sockets.Add(new BoardSocket(new Point(609, 704), new Size(143, 59), "10A"));
            Sockets.Add(new BoardSocket(new Point(607, 760), new Size(139, 72), "40A"));
            Sockets.Add(new BoardSocket(new Point(752, 718), new Size(137, 126), "40A"));
            Sockets.Add(new BoardSocket(new Point(882, 717), new Size(151, 71), "None"));
            Sockets.Add(new BoardSocket(new Point(865, 765), new Size(168, 79), "None"));
            Sockets.Add(new BoardSocket(new Point(1035, 809), new Size(107, 57), "10A"));
            Sockets.Add(new BoardSocket(new Point(1051, 164), new Size(125, 199), "646"));
            Sockets.Add(new BoardSocket(new Point(1013, 577), new Size(154, 253), "None"));






            //Sockets.Add(new BoardSocket(new Point(921, 293), new Size(120, 65)));
            //Sockets.Add(new BoardSocket(new Point(834, 345), new Size(101, 67)));

            string[] dir = Directory.GetDirectories(@"./Img/camera/", "*", SearchOption.TopDirectoryOnly);
            


            for (int i = 0; i < dir.Length; i++)
            {
                string[] file_in_dir = Directory.GetFiles(dir[i], "CPV1.bmp", SearchOption.TopDirectoryOnly);
            
            var source_rotated = Rotate(new Image<Bgr, byte>(@"./Img/board2.bmp"), 82.5);
            source_rotated.Save(@"./Img/source_rotated.bmp");
            var anchor_template = new Image<Bgr, byte>(@"./Img/anchor_template.bmp"); // Image B
            var board = GetBoard(source_rotated, anchor_template);
            board.Save(dir[i]+"/CPV1_CUT.bmp");
                string[] fuse_dir = Directory.GetDirectories(@"./Img/fuses_templates/", "*", SearchOption.TopDirectoryOnly);
                for (int j = 0; j < fuse_dir.Length; j++)
                {
                    string[] fuse_file_in_dir = Directory.GetFiles(fuse_dir[j], "*.bmp", SearchOption.TopDirectoryOnly);
                    var draw = DrawMatches(board, fuse_file_in_dir);
                    var fuse_dir_name = new DirectoryInfo(fuse_dir[j]).Name;
                    draw.Save(dir[i] + "/drawmatch"+ fuse_dir_name+".bmp");
                }
                var json = JsonConvert.SerializeObject(Sockets, Formatting.Indented);
                var dirName = new DirectoryInfo(dir[i]).Name;
                File.WriteAllText(@"./MatchResult/result_PIN-"+ dirName +"-"+ DateTime.Now.ToString("HH-mm-ss") + ".txt", json);
                // Console.ReadKey();   
            }
            

            // List<BoardSocket> Sockets = new List<BoardSocket>();
            //Sockets.Add(new BoardSocket(new Point(1435,1007), new Size(136,140))); //Large 30A left-up
            //Sockets.Add(new BoardSocket(new Point(1552, 1058), new Size(79, 41)));
            //Average();

        }

        public static Image<Bgr, byte> DrawMatches(Image<Bgr, byte> source, string[] fuse_file_in_dir)
        {
            Image<Bgr, byte> imageToShow = source.Copy();
            for (int i = 0; i < fuse_file_in_dir.Length; i++)
            {
            var template = new Image<Bgr, byte>(fuse_file_in_dir[i]);
            var result = source.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
            
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    if (result.Data[y, x, 0] > 0.85)
                    {
                           
                        Point startPoint = new Point(x, y);
                        Rectangle ROI = new Rectangle(startPoint, template.Size);

                            foreach (var item in Sockets)
                            {
                                if ( (ROI.X+ROI.Width) < (item.SocketStart.X+item.SocketSize.Width) &&
                                     (ROI.X) > (item.SocketStart.X) &&
                                     (ROI.Y) > (item.SocketStart.Y) &&
                                     (ROI.Y + ROI.Height) < (item.SocketStart.Y + item.SocketSize.Height)
                                    )
                                {
                                    var ddir = Path.GetDirectoryName(fuse_file_in_dir[i]);
                                    item.Value = new DirectoryInfo(ddir).Name;
                                }
                            }
                            imageToShow.Draw(ROI, new Bgr(Color.Red), 1);
                        

                    }
                }
            }
            }
            return imageToShow;
        }
        public static Image<Bgr, byte> Rotate(Image<Bgr, byte> source, double degree)
        {
            return source.Rotate(degree, new Bgr(System.Drawing.Color.Black));
        }

        public static Image<Bgr, byte> GetBoard(Image<Bgr, byte> source, Image<Bgr, byte> template)
        {
            var result = source.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    if (result.Data[y, x, 0] > 0.9)
                    {
                        Point startPoint = new Point(y, x-350);

                        Image<Bgr, byte> board = source.Copy(new Rectangle(x - 350, y, 1250, 1170));
                        return board;

                    }
                }
            }
            return null;

        }
        public static void Average()
        {

            string[] dir = Directory.GetDirectories(@".\Img\fuses_templates\", "*", SearchOption.TopDirectoryOnly);
            string[] file_in_dir = Directory.GetFiles(dir[0], "*.bmp", SearchOption.TopDirectoryOnly);
            JObject jsonObject = JObject.Parse(File.ReadAllText(@"./fuse_configuration.json"));

            Image<Bgr, byte> source = new Image<Bgr, byte>(@"./Img/board2.bmp"); // Image B
            //source = source.Not();
            Image<Bgr, byte> template;

            Image<Gray, float> imageRoi;



            var r = 82.5;
            var source2 = source.Rotate(r, new Bgr(System.Drawing.Color.Black));

           
                source2 = source.Rotate(r, new Bgr(System.Drawing.Color.Black));
                Image<Bgr, byte> imageToShow = source2.Copy();
                source2.Save(@"./Img/rotate/" + r + "-axis.bmp");

            foreach (var item in file_in_dir)
            {

                template = new Image<Bgr, byte>(item); // Image A

                using (Image<Gray, float> result = source2.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
                {
                    double[] minValues, maxValues;
                    Point[] minLocations, maxLocations;
                    result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                    for (int x = 0; x < result.Height; x++)
                    {
                        for (int y = 0; y < result.Width; y++)
                        {
                            if (result.Data[x, y, 0] > 0.8)
                            {
                                Point startPoint = new Point(y, x);
                                Rectangle ROI = new Rectangle(startPoint, template.Size);
                                //imageRoi = result.Copy(ROI);
                                /*for (int i = 0; i < imageRoi.Size.Width; i++)
                                {
                                    for (int j = 0; j < imageRoi.Size.Height; j++)
                                    {
                                        Console.Write(imageRoi.Data[j, i, 0].ToString("0.00") + " ");
                                    }
                                    Console.WriteLine();
                                }*/
                               //x = x + imageRoi.Size.Height;
                               // y = y + imageRoi.Size.Width;

                                //Console.ReadKey();
                                imageToShow.Draw(ROI, new Bgr(Color.Red), 1);
                                
                            }
                        }
                    }

                }
            }
            imageToShow.Save(@"./Img/result" + r + ".bmp");

            // Show imageToShow in an ImageBox (here assumed to be called imageBox1)
        }

    }
}
