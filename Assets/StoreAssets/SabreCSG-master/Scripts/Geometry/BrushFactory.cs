#if UNITY_EDITOR || RUNTIME_CSG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sabresaurus.SabreCSG
{
	/// <summary>
	/// Provides easy methods for generating brushes (as Polygon sets)
	/// </summary>
	public static class BrushFactory
	{
		/// <summary>
		/// Generates a cube (size 2,2,2)
		/// </summary>
		/// <returns>Polygons to be supplied to a brush.</returns>
		public static Polygon[] GenerateCube()
		{
			Polygon[] polygons = new Polygon[6];

			// Polygons and vertices are created in a clockwise order

			// Polygons are created in this order: back, left, front, right, bottom, top

			// Vertices with those polygons are created in this order: BR, BL, TL, TR (when viewed aligned with the UV)

			// Therefore to move the two bottom vertices of the left face, you would pick the second face (index 1) and 
			// then	manipulate the first two vertices (indexes 0 and 1)

			// Back
			polygons[0] = new Polygon(new Vertex[] {
				new Vertex(new Vector3(-1, -1, 1), new Vector3(0, 0, 1), new Vector2(1,0)),
				new Vertex(new Vector3(1, -1, 1), new Vector3(0, 0, 1), new Vector2(0,0)),
				new Vertex(new Vector3(1, 1, 1), new Vector3(0, 0, 1), new Vector2(0,1)),
				new Vertex(new Vector3(-1, 1, 1), new Vector3(0, 0, 1), new Vector2(1,1)),
			}, null, false, false);

			// Left
			polygons[1] = new Polygon(new Vertex[] {
				new Vertex(new Vector3(-1, -1, -1), new Vector3(-1, 0, 0), new Vector2(1,0)),
				new Vertex(new Vector3(-1, -1, 1), new Vector3(-1, 0, 0), new Vector2(0,0)),
				new Vertex(new Vector3(-1, 1, 1), new Vector3(-1, 0, 0), new Vector2(0,1)),
				new Vertex(new Vector3(-1, 1, -1), new Vector3(-1, 0, 0), new Vector2(1,1)),
			}, null, false, false);

			// Front
			polygons[2] = new Polygon(new Vertex[] {
				new Vertex(new Vector3(1, -1, 1), new Vector3(1, 0, 0), new Vector2(1,0)),
				new Vertex(new Vector3(1, -1, -1), new Vector3(1, 0, 0), new Vector2(0,0)),
				new Vertex(new Vector3(1, 1, -1), new Vector3(1, 0, 0), new Vector2(0,1)),
				new Vertex(new Vector3(1, 1, 1), new Vector3(1, 0, 0), new Vector2(1,1)),
			}, null, false, false);

			// Right
			polygons[3] = new Polygon(new Vertex[] {
				new Vertex(new Vector3(1, -1, -1), new Vector3(0, 0, -1), new Vector2(1,0)),
				new Vertex(new Vector3(-1, -1, -1), new Vector3(0, 0, -1), new Vector2(0,0)),
				new Vertex(new Vector3(-1, 1, -1), new Vector3(0, 0, -1), new Vector2(0,1)),
				new Vertex(new Vector3(1, 1, -1), new Vector3(0, 0, -1), new Vector2(1,1)),
			}, null, false, false);

			// Bottom
			polygons[4] = new Polygon(new Vertex[] {
				new Vertex(new Vector3(-1, -1, -1), new Vector3(0, -1, 0), new Vector2(1,0)),
				new Vertex(new Vector3(1, -1, -1), new Vector3(0, -1, 0), new Vector2(0,0)),
				new Vertex(new Vector3(1, -1, 1), new Vector3(0, -1, 0), new Vector2(0,1)),
				new Vertex(new Vector3(-1, -1, 1), new Vector3(0, -1, 0), new Vector2(1,1)),
			}, null, false, false);

			// Top
			polygons[5] = new Polygon(new Vertex[] {
				new Vertex(new Vector3(-1, 1, -1), new Vector3(0, 1, 0), new Vector2(1,0)),
				new Vertex(new Vector3(-1, 1, 1), new Vector3(0, 1, 0), new Vector2(0,0)),
				new Vertex(new Vector3(1, 1, 1), new Vector3(0, 1, 0), new Vector2(0,1)),
				new Vertex(new Vector3(1, 1, -1), new Vector3(0, 1, 0), new Vector2(1,1)),
			}, null, false, false);

			return polygons;
		}

		/// <summary>
		/// Generates a cylinder of height and radius 2, unlike a prism sides have smooth normals
		/// </summary>
		/// <returns>Polygons to be supplied to a brush.</returns>
		/// <param name="sideCount">Side count for the cylinder.</param>
		public static Polygon[] GenerateCylinder(int sideCount = 20)
		{
			Polygon[] polygons = new Polygon[sideCount * 3];

			float angleDelta = Mathf.PI * 2 / sideCount;

			for (int i = 0; i < sideCount; i++)
			{
				polygons[i] = new Polygon(new Vertex[] 
					{
						new Vertex(new Vector3(Mathf.Sin(i * angleDelta), -1, Mathf.Cos(i * angleDelta)), 
							new Vector3(Mathf.Sin(i * angleDelta), 0, Mathf.Cos(i * angleDelta)), 
							new Vector2(i * (1f/sideCount),0)),
						new Vertex(new Vector3(Mathf.Sin((i+1) * angleDelta), -1, Mathf.Cos((i+1) * angleDelta)), 
							new Vector3(Mathf.Sin((i+1) * angleDelta), 0, Mathf.Cos((i+1) * angleDelta)), 
							new Vector2((i+1) * (1f/sideCount),0)),
						new Vertex(new Vector3(Mathf.Sin((i+1) * angleDelta), 1, Mathf.Cos((i+1) * angleDelta)), 
							new Vector3(Mathf.Sin((i+1) * angleDelta), 0, Mathf.Cos((i+1) * angleDelta)), 
							new Vector2((i+1) * (1f/sideCount),1)),
						new Vertex(new Vector3(Mathf.Sin(i * angleDelta), 1, Mathf.Cos(i * angleDelta)), 
							new Vector3(Mathf.Sin(i * angleDelta), 0, Mathf.Cos(i * angleDelta)), 
							new Vector2(i * (1f/sideCount),1)),
					}, null, false, false);
			}

			Vertex capCenterVertex = new Vertex(new Vector3(0,1,0), Vector3.up, new Vector2(0,0));

			for (int i = 0; i < sideCount; i++)
			{
				Vertex vertex1 = new Vertex(new Vector3(Mathf.Sin(i * angleDelta), 1, Mathf.Cos(i * angleDelta)), Vector3.up, new Vector2(Mathf.Sin(i * angleDelta), Mathf.Cos(i * angleDelta)));
				Vertex vertex2 = new Vertex(new Vector3(Mathf.Sin((i+1) * angleDelta), 1, Mathf.Cos((i+1) * angleDelta)), Vector3.up, new Vector2(Mathf.Sin((i+1) * angleDelta), Mathf.Cos((i+1) * angleDelta)));

				Vertex[] capVertices = new Vertex[] { vertex1, vertex2, capCenterVertex.DeepCopy() };
				polygons[sideCount + i] = new Polygon(capVertices, null, false, false);
			}

			capCenterVertex = new Vertex(new Vector3(0,-1,0), Vector3.down, new Vector2(0,0));

			for (int i = 0; i < sideCount; i++)
			{
				Vertex vertex1 = new Vertex(new Vector3(Mathf.Sin(i * -angleDelta), -1, Mathf.Cos(i * -angleDelta)), Vector3.down, new Vector2(Mathf.Sin(i * angleDelta), Mathf.Cos(i * angleDelta)));
				Vertex vertex2 = new Vertex(new Vector3(Mathf.Sin((i+1) * -angleDelta), -1, Mathf.Cos((i+1) * -angleDelta)), Vector3.down, new Vector2(Mathf.Sin((i+1) * angleDelta), Mathf.Cos((i+1) * angleDelta)));

				Vertex[] capVertices = new Vertex[] { vertex1, vertex2, capCenterVertex.DeepCopy() };
				polygons[sideCount * 2 + i] = new Polygon(capVertices, null, false, false);
			}

			return polygons;
		}

		/// <summary>
		/// Generates a prism of height and radius 2, unlike a cylinder sides have faceted normals
		/// </summary>
		/// <returns>Polygons to be supplied to a brush.</returns>
		/// <param name="sideCount">Side count for the prism.</param>
		public static Polygon[] GeneratePrism(int sideCount)
		{
			Polygon[] polygons = new Polygon[sideCount * 3];

			float angleDelta = Mathf.PI * 2 / sideCount;

			for (int i = 0; i < sideCount; i++)
			{
				Vector3 normal = new Vector3(Mathf.Sin((i+0.5f) * angleDelta), 0, Mathf.Cos((i+0.5f) * angleDelta));
				polygons[i] = new Polygon(new Vertex[] {

					new Vertex(new Vector3(Mathf.Sin(i * angleDelta), -1, Mathf.Cos(i * angleDelta)), 
						normal,
						new Vector2(0,0)),
					new Vertex(new Vector3(Mathf.Sin((i+1) * angleDelta), -1, Mathf.Cos((i+1) * angleDelta)), 
						normal,
						new Vector2(1,0)),
					new Vertex(new Vector3(Mathf.Sin((i+1) * angleDelta), 1, Mathf.Cos((i+1) * angleDelta)), 
						normal,
						new Vector2(1,1)),
					new Vertex(new Vector3(Mathf.Sin(i * angleDelta), 1, Mathf.Cos(i * angleDelta)), 
						normal,
						new Vector2(0,1)),
				}, null, false, false);
			}

			Vertex capCenterVertex = new Vertex(new Vector3(0,1,0), Vector3.up, new Vector2(0,0));

			for (int i = 0; i < sideCount; i++)
			{
				Vertex vertex1 = new Vertex(new Vector3(Mathf.Sin(i * angleDelta), 1, Mathf.Cos(i * angleDelta)), Vector3.up, new Vector2(Mathf.Sin(i * angleDelta), Mathf.Cos(i * angleDelta)));
				Vertex vertex2 = new Vertex(new Vector3(Mathf.Sin((i+1) * angleDelta), 1, Mathf.Cos((i+1) * angleDelta)), Vector3.up, new Vector2(Mathf.Sin((i+1) * angleDelta), Mathf.Cos((i+1) * angleDelta)));

				Vertex[] capVertices = new Vertex[] { vertex1, vertex2, capCenterVertex.DeepCopy() };
				polygons[sideCount + i] = new Polygon(capVertices, null, false, false);
			}

			capCenterVertex = new Vertex(new Vector3(0,-1,0), Vector3.down, new Vector2(0,0));

			for (int i = 0; i < sideCount; i++)
			{
				Vertex vertex1 = new Vertex(new Vector3(Mathf.Sin(i * -angleDelta), -1, Mathf.Cos(i * -angleDelta)), Vector3.down, new Vector2(Mathf.Sin(i * angleDelta), Mathf.Cos(i * angleDelta)));
				Vertex vertex2 = new Vertex(new Vector3(Mathf.Sin((i+1) * -angleDelta), -1, Mathf.Cos((i+1) * -angleDelta)), Vector3.down, new Vector2(Mathf.Sin((i+1) * angleDelta), Mathf.Cos((i+1) * angleDelta)));

				Vertex[] capVertices = new Vertex[] { vertex1, vertex2, capCenterVertex.DeepCopy() };
				polygons[sideCount * 2 + i] = new Polygon(capVertices, null, false, false);
			}

			return polygons;
		}

		[System.Obsolete("Use GeneratePolarSphere instead, or for a more isotropic geometry use GenerateIcoSphere")]
		public static Polygon[] GenerateSphere(int lateralCount = 6, int longitudinalCount = 12)
		{
			return GeneratePolarSphere(lateralCount, longitudinalCount);
		}

		/// <summary>
		/// Generates an ico-sphere of radius 2. Unlike a polar-sphere this has a more even distribution of vertices.
		/// </summary>
		/// <returns>Polygons to be supplied to a brush.</returns>
		/// <param name="iterationCount">Number of times the surface is subdivided, values of 1 or 2 are recommended.</param>
		public static Polygon[] GenerateIcoSphere(int iterationCount)
		{
			// Derived from http://blog.andreaskahler.com/2009/06/creating-icosphere-mesh-in-code.html
			float longestDimension = (1+Mathf.Sqrt(5f)) / 2f;
			Vector3 sourceVector = new Vector3(0, 1, longestDimension);

			// Make the longest dimension 1, so the icosphere fits in a 2,2,2 cube
			sourceVector.Normalize();

			Vertex[] vertices = new Vertex[]
			{
				new Vertex(new Vector3(-sourceVector.y,+sourceVector.z,sourceVector.x), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(sourceVector.y,+sourceVector.z,sourceVector.x), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(-sourceVector.y,-sourceVector.z,sourceVector.x), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(sourceVector.y,-sourceVector.z,sourceVector.x), Vector3.zero, Vector2.zero),

				new Vertex(new Vector3(sourceVector.x,-sourceVector.y,+sourceVector.z), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(sourceVector.x,+sourceVector.y,+sourceVector.z), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(sourceVector.x,-sourceVector.y,-sourceVector.z), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(sourceVector.x,+sourceVector.y,-sourceVector.z), Vector3.zero, Vector2.zero),

				new Vertex(new Vector3(+sourceVector.z,sourceVector.x,-sourceVector.y), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(+sourceVector.z,sourceVector.x,+sourceVector.y), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(-sourceVector.z,sourceVector.x,-sourceVector.y), Vector3.zero, Vector2.zero),
				new Vertex(new Vector3(-sourceVector.z,sourceVector.x,+sourceVector.y), Vector3.zero, Vector2.zero),
			};

			Polygon[] polygons = new Polygon[]
			{
				new Polygon(new Vertex[] { vertices[0].DeepCopy(),vertices[1].DeepCopy(),vertices[7].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[0].DeepCopy(),vertices[5].DeepCopy(),vertices[1].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[0].DeepCopy(),vertices[7].DeepCopy(),vertices[10].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[0].DeepCopy(),vertices[10].DeepCopy(),vertices[11].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[0].DeepCopy(),vertices[11].DeepCopy(),vertices[5].DeepCopy()}, null, false, false),

				new Polygon(new Vertex[] { vertices[7].DeepCopy(),vertices[1].DeepCopy(),vertices[8].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[1].DeepCopy(),vertices[5].DeepCopy(),vertices[9].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[10].DeepCopy(),vertices[7].DeepCopy(),vertices[6].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[11].DeepCopy(),vertices[10].DeepCopy(),vertices[2].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[5].DeepCopy(),vertices[11].DeepCopy(),vertices[4].DeepCopy()}, null, false, false),

				new Polygon(new Vertex[] { vertices[3].DeepCopy(),vertices[2].DeepCopy(),vertices[6].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[3].DeepCopy(),vertices[4].DeepCopy(),vertices[2].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[3].DeepCopy(),vertices[6].DeepCopy(),vertices[8].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[3].DeepCopy(),vertices[8].DeepCopy(),vertices[9].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[3].DeepCopy(),vertices[9].DeepCopy(),vertices[4].DeepCopy()}, null, false, false),

				new Polygon(new Vertex[] { vertices[6].DeepCopy(),vertices[2].DeepCopy(),vertices[10].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[2].DeepCopy(),vertices[4].DeepCopy(),vertices[11].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[8].DeepCopy(),vertices[6].DeepCopy(),vertices[7].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[9].DeepCopy(),vertices[8].DeepCopy(),vertices[1].DeepCopy()}, null, false, false),
				new Polygon(new Vertex[] { vertices[4].DeepCopy(),vertices[9].DeepCopy(),vertices[5].DeepCopy()}, null, false, false),
			};

			// Refine 
			for (int i = 0; i < iterationCount; i++)
			{
				Polygon[] newPolygons = new Polygon[polygons.Length*4];
				for (int j = 0; j < polygons.Length; j++) 
				{
					Vertex a = Vertex.Lerp(polygons[j].Vertices[0], polygons[j].Vertices[1], 0.5f);
					Vertex b = Vertex.Lerp(polygons[j].Vertices[1], polygons[j].Vertices[2], 0.5f);
					Vertex c = Vertex.Lerp(polygons[j].Vertices[2], polygons[j].Vertices[0], 0.5f);

					a.Position = a.Position.normalized;
					b.Position = b.Position.normalized;
					c.Position = c.Position.normalized;

					newPolygons[j*4+0] = new Polygon(new Vertex[] { polygons[j].Vertices[0].DeepCopy(), a.DeepCopy(), c.DeepCopy()}, null, false, false);
					newPolygons[j*4+1] = new Polygon(new Vertex[] { polygons[j].Vertices[1].DeepCopy(), b.DeepCopy(), a.DeepCopy()}, null, false, false);
					newPolygons[j*4+2] = new Polygon(new Vertex[] { polygons[j].Vertices[2].DeepCopy(), c.DeepCopy(), b.DeepCopy()}, null, false, false);
					newPolygons[j*4+3] = new Polygon(new Vertex[] { a.DeepCopy(), b.DeepCopy(), c.DeepCopy()}, null, false, false);
				}
				polygons = newPolygons;
			}

			for (int i = 0; i < polygons.Length; i++) 
			{
                bool anyAboveHalf = false;

                for (int j = 0; j < polygons[i].Vertices.Length; j++)
                {
                    Vector3 normal = polygons[i].Vertices[j].Position.normalized;
                    polygons[i].Vertices[j].Normal = normal;
                    float piReciprocal = 1f / Mathf.PI;
                    float u = 0.5f - 0.5f * Mathf.Atan2(normal.x, -normal.z) * piReciprocal;
                    float v = 1f - Mathf.Acos(normal.y) * piReciprocal;

                    if (Mathf.Abs(u) < 0.01f
                        || Mathf.Abs(1 - Mathf.Abs(u)) < 0.01f)
                    {
                        if (polygons[i].Plane.normal.x > 0)
                        {
                            u = 0;
                        }
                        else
                        {
                            u = 1;
                        }
                    }

                    if(u > 0.75f)
                    {
                        anyAboveHalf = true;
                    }

                    //Debug.Log(u);
                    polygons[i].Vertices[j].UV = new Vector2(u, v);

                    //const float kOneOverPi = 1.0 / 3.14159265;
                    //float u = 0.5 - 0.5 * atan(N.x, -N.z) * kOneOverPi;
                    //float v = 1.0 - acos(N.y) * kOneOverPi;
                }

                if (anyAboveHalf)
                {
                    for (int j = 0; j < polygons[i].Vertices.Length; j++)
                    {
                        Vector2 uv = polygons[i].Vertices[j].UV;
                        if (uv.x < 0.5f)
                        {
                            uv.x += 1;
                        }
                        polygons[i].Vertices[j].UV = uv;

                    }
                }
            }

			return polygons;
		}

		/// <summary>
		/// Generates a sphere of radius 2
		/// </summary>
		/// <returns>Polygons to be supplied to a brush.</returns>
		/// <param name="lateralCount">Vertex count up from the south pole to the north pole.</param>
		/// <param name="longitudinalCount">Vertex count around the sphere equator.</param>
		public static Polygon[] GeneratePolarSphere(int lateralCount = 6, int longitudinalCount = 12)
		{
			Polygon[] polygons = new Polygon[lateralCount * longitudinalCount];

			float angleDelta = 1f / lateralCount;
			float longitudinalDelta = 1f / longitudinalCount;

			// Generate tris for the top and bottom, then quads for the rest
			for (int i = 0; i < lateralCount; i++)
			{
				for (int j = 0; j < longitudinalCount; j++)
				{
					Vertex[] vertices;

					if(i == lateralCount-1)
					{
						vertices = new Vertex[] {

							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
								Mathf.Cos(Mathf.PI * i * angleDelta),
								Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
							),
								new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
									Mathf.Cos(Mathf.PI * i * angleDelta),
									Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
								),
								new Vector2(i * (1f/lateralCount), (j+1) * (1f/longitudinalCount))),
							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
								Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
								Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
							),
								new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
									Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
									Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
								),
								new Vector2((i+1) * (1f/lateralCount), (j+1) * (1f/longitudinalCount))),
							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * j * longitudinalDelta),
								Mathf.Cos(Mathf.PI * i * angleDelta),
								Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * j * longitudinalDelta)
							), 
								new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * j * longitudinalDelta),
									Mathf.Cos(Mathf.PI * i * angleDelta),
									Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * j * longitudinalDelta)
								), 
								new Vector2(i * (1f/lateralCount), j * (1f/longitudinalCount))),
						};
					}
					else if(i > 0)
					{
						vertices = new Vertex[] {

							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
								Mathf.Cos(Mathf.PI * i * angleDelta),
								Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
							),
								new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
									Mathf.Cos(Mathf.PI * i * angleDelta),
									Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
								),
								new Vector2(i * (1f/lateralCount), (j+1) * (1f/longitudinalCount))),
							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
								Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
								Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
							),
								new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
									Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
									Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
								),
								new Vector2((i+1) * (1f/lateralCount), (j+1) * (1f/longitudinalCount))),
							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * j * longitudinalDelta),
								Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
								Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * j * longitudinalDelta)
							), 
								new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * j * longitudinalDelta),
									Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
									Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * j * longitudinalDelta)
								), 
								new Vector2((i+1) * (1f/lateralCount), j * (1f/longitudinalCount))),
							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * j * longitudinalDelta),
								Mathf.Cos(Mathf.PI * i * angleDelta),
								Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * j * longitudinalDelta)
							), 
								new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * j * longitudinalDelta),
									Mathf.Cos(Mathf.PI * i * angleDelta),
									Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * j * longitudinalDelta)
								), 
								new Vector2(i * (1f/lateralCount), j * (1f/longitudinalCount))),
						};
					}
					else // i == 0
					{
						vertices = new Vertex[] {

							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
								Mathf.Cos(Mathf.PI * i * angleDelta),
								Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
							),
								new Vector3(Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
									Mathf.Cos(Mathf.PI * i * angleDelta),
									Mathf.Sin(Mathf.PI * i * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
								),
								new Vector2(i * (1f/lateralCount), (j+1) * (1f/longitudinalCount))),
							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
								Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
								Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
							),
								new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * (j+1) * longitudinalDelta),
									Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
									Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * (j+1) * longitudinalDelta)
								),
								new Vector2((i+1) * (1f/lateralCount), (j+1) * (1f/longitudinalCount))),
							new Vertex(new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * j * longitudinalDelta),
								Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
								Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * j * longitudinalDelta)
							), 
								new Vector3(Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Cos(2 * Mathf.PI * j * longitudinalDelta),
									Mathf.Cos(Mathf.PI * (i+1) * angleDelta),
									Mathf.Sin(Mathf.PI * (i+1) * angleDelta) * Mathf.Sin(2 * Mathf.PI * j * longitudinalDelta)
								), 
								new Vector2((i+1) * (1f/lateralCount), j * (1f/longitudinalCount))),
						};
					}

					for (int d = 0; d < vertices.Length; d++)
					{
						vertices[d].UV = new Vector2(vertices[d].UV.y, 1 - vertices[d].UV.x);
					}

					polygons[i + j * lateralCount] = new Polygon(vertices, null, false, false);
				}
			}

			return polygons;
		}

		/// <summary>
		/// Generates the polygons from a supplied convex mesh, preserving quads if the MeshImporter has <c>keepQuads</c> set.
		/// </summary>
		/// <returns>The polygons converted from the mesh.</returns>
		/// <param name="sourceMesh">Source mesh.</param>
		public static List<Polygon> GeneratePolygonsFromMesh(Mesh sourceMesh)
		{
			List<Polygon> generatedPolygons = new List<Polygon>();
			// Each sub mesh can have a different topology, i.e. triangles and quads
			for (int subMeshIndex = 0; subMeshIndex < sourceMesh.subMeshCount; subMeshIndex++) 
			{
				MeshTopology meshTopology = sourceMesh.GetTopology(subMeshIndex);
				// The vertex count per polygon that we need to walk through the indices at
				int stride = 1;
				if(meshTopology == MeshTopology.Quads)
				{
					stride = 4;
				}
				else if(meshTopology == MeshTopology.Triangles)
				{
					stride = 3;
				}
				else
				{
					Debug.LogError("Unhandled sub mesh topology " + meshTopology + ". Ignoring sub mesh.");
					continue;
				}

				// Grab this sub mesh's index buffer
				int[] indices = sourceMesh.GetIndices(subMeshIndex);

				// Walk through the polygons in the index buffer
				for (int j = 0; j < indices.Length/stride; j++) 
				{
					// Create a new vertex buffer for each polygon
					Vertex[] vertices = new Vertex[stride];

					// Pull out all the vertices for this source polygon
					for (int k = 0; k < stride; k++) 
					{
						int vertexIndex = indices[j*stride+k];

						vertices[k] = new Vertex(sourceMesh.vertices[vertexIndex], 
							sourceMesh.normals[vertexIndex], 
							sourceMesh.uv[vertexIndex]);
					}
					// Generate a new polygon using these vertices and add it to the output polygon list
					Polygon polygon = new Polygon(vertices, null, false, false);
					generatedPolygons.Add(polygon);
				}
			}
			// Finally return all the converted polygons
			return generatedPolygons;
		}

		/// <summary>
		/// Generates a mesh from supplied polygons, particularly useful for visualising a brush's polygons on a MeshFilter
		/// </summary>
		/// <param name="polygons">Polygons.</param>
		/// <param name="mesh">Mesh to be written to. Prior to settings the mesh buffers - if the existing mesh is null it will be set to a new one, otherwise the existing mesh is cleared</param>
		/// <param name="polygonIndices">Maps triangle index (input) to polygon index (output). i.e. int polyIndex = polygonIndices[triIndex];</param>
		public static void GenerateMeshFromPolygons(Polygon[] polygons, ref Mesh mesh, out List<int> polygonIndices)
		{
			if(mesh == null)
			{
				mesh = new Mesh();
			}
			mesh.Clear();
			//	        mesh = new Mesh();
			List<Vector3> vertices = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			List<Color> colors = new List<Color>();
			List<int> triangles = new List<int>();

			// Maps triangle index (input) to polygon index (output). i.e. int polyIndex = polygonIndices[triIndex];
			polygonIndices = new List<int>();

			// Set up an indexer that tracks unique vertices, so that we reuse vertex data appropiately
			VertexList vertexList = new VertexList();

			// Iterate through every polygon and triangulate
			for (int i = 0; i < polygons.Length; i++)
			{
				Polygon polygon = polygons[i];
				List<int> indices = new List<int>();

				for (int j = 0; j < polygon.Vertices.Length; j++)
				{
					// Each vertex must know about its shared data for geometry tinting
					//polygon.Vertices[j].Shared = polygon.SharedBrushData;
					// If the vertex is already in the indexer, fetch the index otherwise add it and get the added index
					int index = vertexList.AddOrGet(polygon.Vertices[j]);
					// Put each vertex index in an array for use in the triangle generation
					indices.Add(index);
				}

				// Triangulate the n-sided polygon and allow vertex reuse by using indexed geometry
				for (int j = 2; j < indices.Count; j++)
				{
					triangles.Add(indices[0]);
					triangles.Add(indices[j - 1]);
					triangles.Add(indices[j]);

					// Map that this triangle is from the specified polygon (so we can map back from triangles to polygon)
					polygonIndices.Add(i);
				}
			}

			// Create the relevant buffers from the vertex array
			for (int i = 0; i < vertexList.Vertices.Count; i++)
			{
				vertices.Add(vertexList.Vertices[i].Position);
				normals.Add(vertexList.Vertices[i].Normal);
				uvs.Add(vertexList.Vertices[i].UV);
				//	                colors.Add(((SharedBrushData)indexer.Vertices[i].Shared).BrushTintColor);
			}

			// Set the mesh buffers
			mesh.vertices = vertices.ToArray();
			mesh.normals = normals.ToArray();
			mesh.colors = colors.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.triangles = triangles.ToArray();
		}
	}
}
#endif