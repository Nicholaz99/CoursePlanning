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
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            semester.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Graph g = new Graph("./input/input.txt");
            List<List<string>> sem = new List<List<string>>();
            //Console.WriteLine("ASU EWE LONTE");
            g.topologicalSort(ref sem, "dfs");
            //var labels = new List<Label> { semester1, semester2, semester3, semester4, semester5 };
            int it = 0;
            semester.Text = "";
            foreach (List<string> ls in sem)
            {
                semester.Visible = true;
                semester.Text = semester.Text + "Semester " + (it+1)+": " + ls[0];
                for (int i=1; i<ls.Count;i++)
                {
                    semester.Text = semester.Text + ", " + ls[i];
                }
                semester.Text = semester.Text + "\n";
                it++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button1.Height;
            SidePanel.Top = button1.Top;
            button3.Visible = false;
            button6.Visible = true;
            button5.Visible = true;
            label2.Text = "";
            label1.Visible = false;
            label3.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            semester.Visible = false;
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
            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = true;
            semester.Visible = false;
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
            button5.Visible = true;
            label1.Visible = false;
            label3.Visible = false;
            label2.Text = "";
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            semester.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Graph g = new Graph("./input/input.txt");
            g.initGraph();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Graph g = new Graph("./input/input.txt");
            List<List<string>> sem = new List<List<string>>();
            g.topologicalSort(ref sem, "bfs");
            int it = 0;
            semester.Text = "";
            foreach (List<string> ls in sem)
            {
                semester.Visible = true;
                semester.Text = semester.Text + "Semester " + (it + 1) + ": " + ls[0];
                for (int i = 1; i < ls.Count; i++)
                {
                    semester.Text = semester.Text + ", " + ls[i];
                }
                semester.Text = semester.Text + "\n";
                it++;
            }
        }
    }

    class Graph
    {
        private Dictionary<string, HashSet<string>> graph;
        private Dictionary<string, bool> visited = new Dictionary<string, bool>();
        private List<string>[] mat;
        private int no_of_vertex;

        public Graph(string file)
        {
            /* Read input.txt and store to matrix */
            string input = System.IO.File.ReadAllText(file);
            string[] lines = input.Split('\n');
            List<string>[] matrix = new List<string>[lines.Length];

            /* Initialize all the list in matrix */
            for (int i = 0; i < lines.Length; i++)
            {
                matrix[i] = new List<string>();
            }

            /* Add each element to list */
            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Remove(lines[i].Length - 1).Split(',');
                foreach (string s in temp)
                {
                    matrix[i].Add(s);
                }
            }

            mat = matrix;

            no_of_vertex = matrix.Length;

            graph = new Dictionary<string, HashSet<string>>(); /* Initialize matrix to zeros */
            for (int i = 0; i < no_of_vertex; i++)
            {
                graph[matrix[i][0]] = new HashSet<string>();
                for (int j = 1; j < matrix[i].Count; j++)
                {
                    graph[matrix[i][0]].Add(matrix[i][j]);
                }
            }
        }

        public void resetGraph()
        {
            graph = new Dictionary<string, HashSet<string>>(); /* Initialize matrix to zeros */
            for (int i = 0; i < no_of_vertex; i++)
            {
                graph[mat[i][0]] = new HashSet<string>();
                for (int j = 1; j < mat[i].Count; j++)
                {
                    graph[mat[i][0]].Add(mat[i][j]);
                }
            }
        }

        private void deleteVertex(string vertex)
        {
            graph.Remove(vertex);
            foreach (KeyValuePair<string, HashSet<string>> kvp in graph)
            {
                kvp.Value.Remove(vertex);
            }
        }

        private bool visitedAllPrereq(string vertex)
        {
            bool visitedAll = true;
            foreach (string str in graph[vertex])
            {
                visitedAll = visitedAll && visited[str];
            }
            return visitedAll;
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
                if (kvp.Value.Count == 0 && !visited[kvp.Key])
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

            bool found = false;
            foreach (string i in graph.Keys)
            {
                if (isAdjacent(vertex, i) && !visited[i])
                {
                    DFS(i, ref list, ref timestamp, ref vertexes);
                    found = true;
                }
            }
            if (!found)
            {
                foreach (string i in graph.Keys)
                {
                    if (!visited[i] && visitedAllPrereq(i))
                    {
                        DFS(i, ref list, ref timestamp, ref vertexes);
                        break;
                    }
                }
            }

            /* Finished timestamp */
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
                Boolean hasCouple = false;
                foreach (string j in kvp.Value)
                {
                    hasCouple = true;
                    graf.AddEdge(j, kvp.Key);
                }
                if (!hasCouple)
                {
                    graf.AddNode(kvp.Key);
                }
                
            }
            viewer.Graph = graf;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(444, 178);
            form.Size = new Size(845, 300);
            form.ResumeLayout();
            form.BringToFront();
            form.Show();
        }

        private void BFS(ref List<string> list, ref List<string> vertexes)
        {
            Queue<string> q = new Queue<string>();
            foreach (KeyValuePair<string, HashSet<string>> kvp in graph)
            {
                if (noInEdge(kvp.Key))
                {
                    q.Enqueue(kvp.Key);
                }
            }
            // q.Enqueue(vertex);
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

        bool prereqTaken(string s, ref Dictionary<string, bool> taken)
        {
            bool takenAll = true;
            foreach (string str in graph[s])
            {
                takenAll = takenAll && taken[str];
            }
            return takenAll;
        }

        public void topologicalSort(ref List<List<string>> sem, string chooseRoute)
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
                Boolean hasCouple = false;
                foreach (string j in kvp.Value)
                {
                    hasCouple = true;
                    graf.AddEdge(j, kvp.Key);
                }
                if (!hasCouple)
                {
                    graf.AddNode(kvp.Key);
                }

            }
            viewer.Graph = graf;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(444, 178);
            form.Size = new Size(845, 300);
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
                    var taskwait = Task.Delay(100);
                    taskwait.Wait();
                }
                listDFS.Sort(compareTuple);

                Dictionary<string, bool> taken = new Dictionary<string, bool>();

                foreach(string vertex in graph.Keys)
                {
                    taken[vertex] = false;
                }

                List<string> list;
                int num_taken = 0;

                while(num_taken != no_of_vertex)
                {
                    Console.WriteLine("asdfasdf");
                    list = new List<string>();
                    int idx = 0;
                    while (idx < listDFS.Count)
                    {
                        if(prereqTaken(listDFS[idx].Item2, ref taken) && !list.Contains(listDFS[idx].Item2))
                        {
                            list.Add(listDFS[idx].Item2);
                            string temp = listDFS[idx].Item2;
                            for(int iasdf = 0; iasdf < listDFS.Count; iasdf++)
                            {
                                if(listDFS[iasdf].Item2 == temp)
                                {
                                    listDFS.RemoveAt(iasdf);
                                }
                            }
                            num_taken++;
                        }
                        else
                        {
                            idx++;
                        }
                        if(num_taken == no_of_vertex)
                        {
                            break;
                        }


                    }
                    foreach(string s in list)
                    {
                        taken[s] = true;
                    }
                    sem.Add(list);
                }

                foreach(List<string> asdf in sem)
                {
                    Console.WriteLine("-----");
                    foreach(string asdfa in asdf)
                    {
                        Console.WriteLine(asdfa);
                    }
                }
            }
            
            if (chooseRoute == "bfs")
            {
                List<string> listBFS = new List<string>();
                initializeVisited();
                List<string> vertexes = new List<string>();
                BFS(ref listBFS, ref vertexes);
                for (int i = 0; i < vertexes.Count; i++)
                {
                    //form.Close();
                    graf.FindNode(vertexes[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
                    form.Refresh();
                    var taskwait = Task.Delay(100);
                    taskwait.Wait();
                }
                resetGraph();
                Dictionary<string, bool> taken = new Dictionary<string, bool>();

                foreach (string str in listBFS)
                {
                    taken[str] = false;
                }

                List<string> list;
                int num_taken = 0;
                while (num_taken != no_of_vertex)
                {
                    Console.WriteLine("asdfasdf");
                    list = new List<string>();
                    int idx = 0;
                    while (idx < listBFS.Count)
                    {
                        if (prereqTaken(listBFS[idx], ref taken) && !list.Contains(listBFS[idx]))
                        {
                            list.Add(listBFS[idx]);
                            string temp = listBFS[idx];
                            for (int iasdf = 0; iasdf < listBFS.Count; iasdf++)
                            {
                                if (listBFS[iasdf] == temp)
                                {
                                    listBFS.RemoveAt(iasdf);
                                }
                            }
                            num_taken++;
                        }
                        else
                        {
                            idx++;
                        }
                        if (num_taken == no_of_vertex)
                        {
                            break;
                        }


                    }
                    foreach (string s in list)
                    {
                        taken[s] = true;
                    }
                    sem.Add(list);
                }
                /*while (listBFS.Count != 0)
                {
                    t = new List<string>();
                    while (listBFS.Count != 0 && noInEdge(listBFS[0]))
                    {
                        t.Add(listBFS[0]);
                        listBFS.RemoveAt(0);
                    }
                    foreach (string s in t)
                    {
                        deleteVertex(s);
                    }
                    sem.Add(t);
                }*/
            }
        }
    }
}

