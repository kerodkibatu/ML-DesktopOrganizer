# ML-DesktopOrganizer

This repository provides an automated solution for organizing desktop icons using AI/ML techniques. The project aims to automate the process of arranging desktop icons based on their semantic meaning and contextual understanding. The methodology and technical aspects of this project are outlined below.

## Table of Contents
1. [Initial Concept and Objective](#initial-concept-and-objective)
2. [Feature Extraction](#feature-extraction)
3. [Embedding and Contextual Understanding](#embedding-and-contextual-understanding)
4. [Dimensionality Reduction](#dimensionality-reduction)
5. [Desktop Management](#desktop-management)

## Initial Concept and Objective
The objective of this project is to automate the organization of desktop icons using AI/ML techniques. The initial idea is to utilize t-SNE (t-distributed Stochastic Neighbor Embedding), a dimensionality reduction algorithm, to map high-dimensional vectors to a lower-dimensional representation while preserving local similarity.

## Feature Extraction
The following features are considered for organizing the desktop icons:
- Icon names
- File names and types (folder/file)
- Full path of the file

## Embedding and Contextual Understanding
To represent the desktop icons in a latent space, word embedding techniques are employed. Word embedding allows the representation of documents and words using numeric vectors, enabling words with similar meanings to have similar representations. In the context of desktop icons, a Large Language Model (LLM) is used to describe the icon by providing the name, type (folder/file), and full path. This information is bundled with the LLM explanation and then embedded to accurately represent icons in a higher-level, lower-dimensional space.

## Dimensionality Reduction
The challenge of dealing with embedding vectors consisting of over 780 floating-point numbers is addressed by using t-SNE. This algorithm remaps the high-dimensional latent space into just two dimensions while preserving the structure of the original space.

## Desktop Management
Manipulating desktop icons using the Windows Desktop API can be challenging due to poor documentation. To overcome this challenge, a premade C# library called DesktopIconsManipulator (available on NuGet) is utilized. This library provides a convenient interface for handling low-level desktop management tasks.

For more details on the implementation and usage of the ML-DesktopOrganizer, please refer to the documentation and source code in this repository.