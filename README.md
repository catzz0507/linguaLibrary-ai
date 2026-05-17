# LinguaLibrary AI

> A Gemma 4-powered multilingual language learning platform, starting with JLPT Japanese.

---

## Overview

LinguaLibrary AI is a game-based language learning platform that combines RPG gameplay with AI-generated educational content.

The current prototype focuses on JLPT Japanese learning, where players answer AI-generated quiz questions during real-time battle encounters.

Instead of relying on static worksheets or repetitive flashcards, LinguaLibrary AI transforms language practice into an interactive gameplay loop.

---

## The Problem

Language learners often face several challenges:

- Repetitive and static practice materials
- Limited access to tutors or premium platforms
- Low engagement and motivation
- Dependence on constant internet connectivity
- Lack of adaptive learning experiences

Traditional quiz apps rarely feel immersive or rewarding.

---

## Our Solution

LinguaLibrary AI uses Gemma 4 to dynamically generate structured language-learning questions and integrate them directly into gameplay.

Players:

- Solve JLPT-style questions
- Select answers during battle
- Trigger combat actions through correct responses
- Learn through repeated gameplay interaction

The result is a more engaging and scalable learning experience.

---

## Why Gemma 4

Gemma 4 enables:

- Local AI inference
- Structured educational content generation
- Flexible multilingual expansion
- Reduced dependency on cloud-only systems
- Potential offline learning workflows

Instead of manually authoring thousands of questions, the system generates quiz content dynamically through prompt-driven generation.

---

## Current Prototype: JLPT Battle Mode

The current implementation supports:

- JLPT-style Japanese quizzes
- Multiple question categories
- Four-choice answer system
- RPG battle gameplay
- Quiz queue system
- Prompt-based question generation
- Structured quiz parsing
- Unity-based game UI

### Supported Question Categories

- Kanji Reading
- Vocabulary
- Grammar
- Usage
- Sentence Assembly
- Context Understanding
- Synonym Selection
- Text Grammar

---

## Long-Term Vision

JLPT is only the first module.

The long-term goal is to evolve LinguaLibrary AI into a unified multilingual learning platform supporting:

- Korean TOPIK
- English TOEIC / IELTS
- Chinese HSK
- Spanish DELE
- French DELF
- German Goethe-Zertifikat

The architecture separates:

- Language profiles
- Prompt templates
- Quiz validation
- Gameplay systems

This allows future language modules to reuse the same AI-powered learning pipeline.

---

## Architecture

```text
Unity Game
    ↓
QuizProvider
    ↓
PromptBuilder
    ↓
Gemma 4 API
    ↓
JSON Parser / Validator
    ↓
Quiz Queue
    ↓
Battle Gameplay
```

---

## Example Gameplay Loop

```text
Question Generated
    ↓
Player Selects Answer
    ↓
Battle Action Triggered
    ↓
Correct / Incorrect Feedback
    ↓
Next Question Loaded
```

---

## Tech Stack

- Unity 6
- C#
- Gemma 4
- Local LLM API workflow
- JSON-based structured parsing

---

## Screenshots

### Gameplay

![Gameplay](docs/screenshots/gameplay_full.png)

### Quiz Screen

![Quiz](docs/screenshots/quiz_screen.png)

### Battle Feedback

![Battle](docs/screenshots/battle_correct.png)

---

## Current Development Status

Current state:

- Prototype completed
- GitHub repository prepared
- Gameplay loop implemented
- Hackathon submission materials in progress

Planned next steps:

- Adaptive difficulty system
- Additional language modules
- Audio and pronunciation training
- Multiplayer learning features
- Teacher dashboard
- Mobile deployment

---

## Setup

### Requirements

- Unity 6
- Local Gemma 4-compatible API server

### Run Instructions

1. Clone this repository
2. Open the project in Unity
3. Configure local API endpoint
4. Launch the main gameplay scene
5. Start learning through battle gameplay

---

## Repository Structure

```text
Assets/
Packages/
ProjectSettings/
docs/
└─ screenshots/
```

---

## Hackathon Submission

This project is being prepared for the Gemma 4 Hackathon under the categories:

- Future of Education
- Digital Equity & Inclusivity

---

## License

MIT
