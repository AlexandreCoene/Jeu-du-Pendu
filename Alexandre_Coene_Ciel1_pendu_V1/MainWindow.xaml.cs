using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Alexandre_Coene_Ciel1_pendu_V1 // Espace de nommage du projet
{
    public partial class MainWindow : Window // Classe partielle MainWindow
    {
        private string[] wordList = { "PROGRAMME", "PENDU", "MONITEUR", "PRISE", "LOGICIEL", "SOURIS", "CLAVIER", "CODE" }; // Liste des mots
        private string selectedWord; // Mot sélectionné
        private int maxAttempts = 7; // Nombre de tentatives maximum
        private int attemptsLeft; // Tentatives restantes
        private char[] guessedWord; // Mot deviné
        private DispatcherTimer gameTimer; // Minuteur
        private TimeSpan elapsedTime; // Temps écoulé

        public MainWindow() // Constructeur de la classe MainWindow
        {
            InitializeComponent(); // Initialise les composants de l'interface utilisateur

            BTN_Play.Click += StartGame; // Associe l'événement au bouton BTN_Play
            BTN_Stop.Click += StopGame; // Associe l'événement au bouton BTN_Stop

            gameTimer = new DispatcherTimer(); // Crée un nouveau minuteur
            gameTimer.Interval = TimeSpan.FromSeconds(1); // Définit l'intervalle du minuteur à 1 seconde
            gameTimer.Tick += GameTimer_Tick; // Associe l'événement au minuteur
        }



        // -------------------------------------------------------------------------- Start Game -----------------------------------------------------------------------------------
        private void StartGame(object sender, RoutedEventArgs e) // Démarre le jeu
        {
            attemptsLeft = maxAttempts; // Initialise le nombre de tentatives restantes
            IMG_pendu.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Image/Pendu 1.png", UriKind.Relative)); // Initialise l'image du pendu
            selectedWord = wordList[new Random().Next(wordList.Length)]; // Sélectionne un mot aléatoire
            guessedWord = new char[selectedWord.Length]; // Initialise le mot deviné

            for (int i = 0; i < selectedWord.Length; i++) // Parcours le mot sélectionné
            {
                guessedWord[i] = '_'; // Initialise le mot deviné
            }
            UpdateWordLabel(); // Met à jour le libellé du mot
            EnableAlphabetButtons(true); // Active les boutons de l'alphabet
            ResetButtonStyles(); // Réinitialise le style des boutons par défaut
            elapsedTime = TimeSpan.Zero; // Initialise le temps écoulé
            gameTimer.Start(); // Démarre le minuteur
        }



        // -------------------------------------------------------------------------- Stop Game ----------------------------------------------------------------------------------
        private void StopGame(object sender, RoutedEventArgs e) // Arrête le jeu
        {
            MessageBox.Show("Game Over! Le mot était : " + selectedWord, "Game Over", MessageBoxButton.OK, MessageBoxImage.Information); // Affiche un message de défaite
            EnableAlphabetButtons(false); // Désactive les boutons de l'alphabet
            gameTimer.Stop(); // Arrête le minuteur
        }



        // -------------------------------------------------------------------------- Event / Boutons ----------------------------------------------------------------------------
        private void BTN_Click(object sender, RoutedEventArgs e) // Gère l'événement Click des boutons
        {
            Button clickedButton = (Button)sender; // Récupère le bouton cliqué
            clickedButton.IsEnabled = false; // Désactive le bouton cliqué
            char guessedLetter = clickedButton.Content.ToString()[0]; // Récupère la lettre devinée

            if (selectedWord.Contains(guessedLetter))
            {
                UpdateGuessedWord(guessedLetter); // Met à jour le mot deviné
            }
            else
            {
                HandleIncorrectGuess(); // Gère une devinette incorrecte
            }
            UpdateWordLabel(); // Met à jour le libellé du mot
        }



        // -------------------------------------------------------------------------- Met à jour le mot deviné -------------------------------------------------------------------
        private void UpdateGuessedWord(char guessedLetter) // Met à jour le mot deviné
        {
            for (int i = 0; i < selectedWord.Length; i++) // Parcours le mot sélectionné
            {
                if (selectedWord[i] == guessedLetter) // Vérifie si la lettre devinée est dans le mot
                {
                    guessedWord[i] = guessedLetter; // Met à jour le mot deviné
                }
            }
        }





        // -------------------------------------------------------------------------- Enlève une vie et utilise la fonction update de l'image ------------------------------------
        private void HandleIncorrectGuess() // Gère une devinette incorrecte
        {
            attemptsLeft--; // Décrémente le nombre de tentatives restantes
            UpdateHangmanImage(); // Met à jour l'image du pendu
        }





        // -------------------------------------------------------------------------- Ajout de la lettre au mot ------------------------------------------------------------------ 
        private void UpdateWordLabel() // Met à jour le libellé du mot
        {
            Mot_Trouver.Content = "Mot à trouver : " + new string(guessedWord); // Met à jour le libellé du mot
        }





        // -------------------------------------------------------------------------- Update Image ------------------------------------------------------------------------------- 
        private void UpdateHangmanImage() // Met à jour l'image du pendu
        {
            int imageIndex = maxAttempts - attemptsLeft + 1; // Calcule l'index de l'image à afficher
            if (imageIndex >= 1 && imageIndex <= maxAttempts) // Vérifie si l'index est valide
            {
                // IMG_1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri($"/Alexandre_Coene_Ciel1_pendu_V1;/Image/ {imageIndex}.png", UriKind.Relative)); // Met à jour l'image du pendu
                IMG_pendu.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri($"/Image/Pendu {imageIndex}.png", UriKind.Relative)); // Met à jour l'image du pendu
            }
        }





        // -------------------------------------------------------------------------- Start Game ---------------------------------------------------------------------------------
        private void EnableAlphabetButtons(bool enable) // Active ou désactive les boutons de l'alphabet
        {
            for (int i = 0; i < Grid_Middle.Children.Count; i++) // Parcours les boutons
            {
                UIElement element = Grid_Middle.Children[i]; // Récupère le bouton

                if (element is Button) // Vérifie si le bouton est un bouton
                {
                    ((Button)element).IsEnabled = enable; // Active ou désactive les boutons de l'alphabet
                }
            }
        }



        // -------------------------------------------------------------------------- Remettre l'opacité des boutons ---------------------------------------------------------------
        private void ResetButtonStyles() // Réinitialise le style des boutons par défaut
        {
            int count = Grid_Middle.Children.Count; // Récupère le nombre de boutons
            for (int i = 0; i < count; i++) // Parcours les boutons
            {
                UIElement element = Grid_Middle.Children[i]; // Récupère le bouton

                if (element is Button) // Vérifie si le bouton est un bouton
                {
                    ((Button)element).IsEnabled = true; // Active le bouton
                  //  ((Button)element).Style = (Style)FindResource("MaterialDesignFlatButton"); // Remet le style du bouton par défaut
                }
            }
        }



        // -------------------------------------------------------------------------- Arrêt et incrémentation du Timer -------------------------------------------------------------
        private void GameTimer_Tick(object sender, EventArgs e) // Gère le minuteur
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1)); // Incrémente le temps écoulé
            ElapsedTimeLabel.Content = "Temps écoulé : " + elapsedTime.ToString(@"mm\:ss"); // Met à jour le libellé pour afficher le temps écoulé
            if (!guessedWord.Contains('_')) // Vérifie si le mot a été deviné
            {
                MessageBox.Show("Félicitations ! Vous avez deviné le mot : " + selectedWord + "\nTemps écoulé : " + elapsedTime.ToString(@"mm\:ss"), "Vous avez gagné !", MessageBoxButton.OK, MessageBoxImage.Information); // Affiche un message de victoire
                EnableAlphabetButtons(false); // Désactive les boutons de l'alphabet
                gameTimer.Stop(); // Arrête le minuteur
                foreach (Button btn in Bouton_Grid.Children.OfType<Button>()) // Parcours les boutons
                {
                    btn.IsEnabled = true; // Reactive les bouton (enlève l'opacity)
                }
            }
            if (attemptsLeft == 0) // Vérifie si le joueur a perdu
            {
                MessageBox.Show("Game Over ! Le mot était : " + selectedWord + "\nTemps écoulé : " + elapsedTime.ToString(@"mm\:ss"), "Game Over", MessageBoxButton.OK, MessageBoxImage.Information); // Affiche un message de défaite
                EnableAlphabetButtons(false); // Désactive les boutons de l'alphabet
                gameTimer.Stop(); // Arrête le minuteur
                foreach (Button btn in Bouton_Grid.Children.OfType<Button>()) // Parcours les boutons
                {
                    btn.IsEnabled = true; // Reactive les bouton (enlève l'opacity)
                } 
            }
        }
    }
}