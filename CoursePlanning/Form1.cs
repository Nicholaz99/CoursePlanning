using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tubes2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Visible = false;
            button3.Visible = false;
            label3.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Graph g = new Graph("./input/input.txt");
            string ans = "";
            g.topologicalSort(ref ans, "dfs");
            label2.Text = "Answer with DFS = " + ans;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button1.Height;
            SidePanel.Top = button1.Top;
            button3.Visible = false;
            button6.Visible = true;
            label2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button2.Height;
            SidePanel.Top = button2.Top;
            button6.Visible = false;
            button3.Visible = false;
            button5.Visible = false;
            label2.Text = "";
            label1.Visible = true;
            label3.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button4.Height;
            SidePanel.Top = button4.Top;
            button6.Visible = false;
            button3.Visible = true;
            label2.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Graph g = new Graph("./input/input.txt");
            g.initGraph();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Graph g = new Graph("./input/input.txt");
            string ans = "";
            g.topologicalSort(ref ans, "bfs");
            label2.Text = "Answer with BFS = " + ans;
        }
    }

    class Graph
    {
        private Dictionary<string, HashSet<string>> graph;
        private Dictionary<string, bool> visited = new Dictionary<string, bool>();
        private int no_of_vertex;

        public Graph(string file)
        {
            // Read input.txt and store to matrix
            string input = System.IO.File.ReadAllText(file);
            string[] lines = input.Split('\n');
            List<string>[] matrix = new List<string>[lines.Length];

            // Initialize all the list in matrix
            for (int i = 0; i < lines.Length; i++)
            {
                matrix[i] = new List<string>();
            }

            // Add each element to list
            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Remove(lines[i].Length - 1).Split(',');
                foreach (string s in temp)
                {
                    matrix[i].Add(s);
                }
            }

            no_of_vertex = matrix.Length;

            graph = new Dictionary<string, HashSet<string>>(); // Initialize matrix to zeros
            for (int i = 0; i < no_of_vertex; i++)
            {
                graph[matrix[i][0]] = new HashSet<string>();
                for (int j = 1; j < matrix[i].Count; j++)
                {
                    graph[matrix[i][0]].Add(matrix[i][j]);
                }
            }
        }

        private void initializeVisited()
        {
            foreach (string s in graph.Keys)
            {
                visited[s] = false;
            }
        }

        private bool isAdjacent(string src, string dest)
        {
            return graph[dest].Contains(src);
        }

        private void deleteEdge(string src, string dest)
        {
            graph[dest].Remove(src);
        }

        private bool visitedAllVertex()
        {
            bool visited_all = true;
            foreach (string i in graph.Keys)
            {
                if (!visited[i])
                {
                    visited_all = false;
                    break;
                }
            }
            return visited_all;
        }

        private int compareTuple(Tuple<int, string> x, Tuple<int, string> y)
        {
            return x.Item1 > y.Item1 ? -1 : 1;
        }

        private string vertexNoIn()
        {
            string res = "\0";
            foreach (KeyValuePair<string, HashSet<string>> kvp in graph)
            {
                if (kvp.Value.Count == 0)
                {
                    res = kvp.Key;
                    break;
                }
            }
            return res;
        }

        private bool noOutEdge(string v)
        {
            bool no_out_edge = true;
            foreach (string i in graph.Keys)
            {
                if (isAdjacent(v, i))
                {
                    no_out_edge = false;
                    break;
                }
            }
            return no_out_edge;
        }

        private bool noInEdge(string v)
        {
            bool no_in_edge = true;
            foreach (string i in graph.Keys)
            {
                if (isAdjacent(i, v))
                {
                    no_in_edge = false;
                    break;
                }
            }
            return no_in_edge;
        }

        private void DFS(string vertex, ref List<Tuple<int, string>> list, ref int timestamp, ref List<string> vertexes)
        {
            visited[vertex] = true;
            vertexes.Add(vertex);
            
            // Start timestamp
            list.Add(Tuple.Create(timestamp, vertex));
            timestamp++;
            foreach (string i in graph.Keys)
            {
                if (isAdjacent(vertex, i) && !visited[i])
                {
                    DFS(i, ref list, ref timestamp, ref vertexes);
                }
            }
            // Finished timestamp
            list.Add(Tuple.Create(timestamp, vertex));
            timestamp++;
        }

        public void initGraph()
        {
            Form form = new Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graf = new Microsoft.Msagl.Drawing.Graph("graf");
            graf.Attr.LayerDirection = Microsoft.Msagl.Drawing.LayerDirection.LR;
            //create the graph content
            foreach (KeyValuePair<string, HashSet<string>> kvp in graph)
            {
                foreach (string j in kvp.Value)
                {
                    graf.AddEdge(j, kvp.Key);
                }
            }
            viewer.Graph = graf;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(444, 300);
            form.Size = new Size(830, 300);
            form.ResumeLayout();
            form.BringToFront();
            form.Show();
        }

        private void BFS(string vertex, ref List<string> list, ref List<string> vertexes)
        {
            Queue<string> q = new Queue<string>();
            q.Enqueue(vertex);
            while ((q.Count != 0) && !visitedAllVertex())
            {
                string v = q.Dequeue();
                if (noInEdge(v) && !visited[v])
                {
                    visited[v] = true;
                    vertexes.Add(v);
                    foreach (string i in graph.Keys)
                    {
                        if (isAdjacent(v, i) && !visited[i])
                        {
                            deleteEdge(v, i);
                            q.Enqueue(i);
                        }
                    }
                    list.Add(v);
                }
            }
        }

        public void topologicalSort(ref string ans, string chooseRoute)
        {
            Form form = new Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graf = new Microsoft.Msagl.Drawing.Graph("graf");
            //create the graph content
            graf.Attr.LayerDirection = Microsoft.Msagl.Drawing.LayerDirection.LR;
            foreach (KeyValuePair<string, HashSet<string>> kvp in graph)
            {
                foreach (string j in kvp.Value)
                {
                    graf.AddEdge(j, kvp.Key);
                }
            }
            viewer.Graph = graf;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(444, 300);
            form.Size = new Size(830, 300);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.ResumeLayout();
            form.Show();

            // Tuple of timestamp and vertex
           
            if (chooseRoute == "dfs")
            {
                List<Tuple<int, string>> listDFS = new List<Tuple<int, string>>();
                initializeVisited();
                int timestamp = 1;
                List<string> vertexes = new List<string>();
                DFS(vertexNoIn(), ref listDFS, ref timestamp, ref vertexes);
                for (int i = 0; i < vertexes.Count; i++)
                {
                    //form.Close();
                    graf.FindNode(vertexes[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
                    form.Refresh();
                    var t = Task.Delay(1000);
                    t.Wait();
                }
                listDFS.Sort(compareTuple);

                // Print all ordered
                string temp = "";
                HashSet<string> printed = new HashSet<string>();
                for (int i = 0; i < listDFS.Count; i++)
                {
                    if (!printed.Contains(listDFS[i].Item2))
                    {
                        printed.Add(listDFS[i].Item2);
                        temp = temp + (listDFS[i].Item2) + " ";
                    }
                }
                ans = temp;
            }
            
            if (chooseRoute == "bfs")
            {
                List<string> listBFS = new List<string>();
                initializeVisited();
                List<string> vertexes = new List<string>();
                BFS(vertexNoIn(), ref listBFS, ref vertexes);
                for (int i = 0; i < vertexes.Count; i++)
                {
                    //form.Close();
                    graf.FindNode(vertexes[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
                    form.Refresh();
                    var t = Task.Delay(1000);
                    t.Wait();
                }

                // Print all Ordered
                string temp = "";
                for (int i = 0; i < listBFS.Count; i++)
                {
                    temp = temp + (listBFS[i] + " ");
                }
                ans = temp;
            }
        }
    }
}

