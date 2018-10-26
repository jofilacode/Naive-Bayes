using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class naive_Bayes
{
    Sentiment_Api data = new Sentiment_Api();
    private List<string> positive = new List<string>();
    private List<string> negative = new List<string>();
    private string status;
    private bool pos_found = false;
    private bool neg_found = false;
   private double total_trainee_set;
    private double total_positive_count;
   private double total_negative_count;
    int positive_yes = 1;
    int positive_no = 0;
    int negative_yes = 1;
    int negative_no = 0;
    private int pos_count;
    private int neg_count;
    private double posterior_probabilty_positive = 1;
    private double posterior_probabiity_negative = 1;
    string classification_status = "unknown";
   
   
	public naive_Bayes()
	{
        total_trainee_set = data.get_total_record_count();
        total_positive_count = data.get_Sentiments_count("positive");
        total_negative_count = data.get_Sentiments_count("negative");
        positive = data.get_Sentiments_Data_P();
        negative = data.get_Sentiments_Data_N();


	}


    public string msg()
    {
        return status;
    }

    private IEnumerable<string> getText(string comments)
    {
        var text = comments;
        var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
        var words = text.Split().Select(x => x.Trim(punctuation));
        return words;
    }

    private double probability_of_positivity()
    {
        double x = ((total_positive_count) / (total_trainee_set));
        return x;
    }


    private double probability_of_negativity()
    {
        double y = ((total_negative_count) / (total_trainee_set));
        return y;
    }



    private double prob_likelihood_positive_Y()
    {


        double prob_w_c = (positive_yes + 1) / (total_positive_count + total_trainee_set);

        return prob_w_c;
    }
    private double prob_likelihood_positive_N()
    {
      
        double prob_w_c = ( positive_no + 1) / (total_positive_count + total_trainee_set);

        return prob_w_c;
    }

     private double prob_likelihood_negative_Y()
    {

        double prob_w_c = (negative_yes + 1) / (total_negative_count + total_trainee_set);

        return prob_w_c;
    }
    private double prob_likelihood_negative_N()
    {

        double prob_w_c = (negative_no + 1) / (total_negative_count + total_trainee_set);

        return prob_w_c;
    }

    public double find_positive_likelihood(string comments)
    {
     
        foreach (string word in getText(comments))
        {
            if (positive.Contains(word))
            {
                pos_count++;
                pos_found = true;
            }
            else if (negative.Contains(word))
            {
                neg_count++;
                posterior_probabilty_positive = (prob_likelihood_positive_N() * probability_of_positivity() * neg_count);

            }
           
           
        }

        return cal_Prob_pos(pos_found) * pos_count;
    }

    public double find_negative_likelihood(string comments)
    {
  
        foreach (string word in getText(comments))
        {
            if (negative.Contains(word))
            {
                neg_count++;
                neg_found = true;
            }
            else if (positive.Contains(word))
            {
                pos_count++;
                posterior_probabiity_negative = (prob_likelihood_negative_N() * probability_of_negativity() * pos_count); 

            }
            
            
        }


        return cal_Prob_Neg(neg_found) * neg_count;
    }

    private double cal_Prob_Neg(bool found_x)
    {
           posterior_probabiity_negative = found_x == true? posterior_probabiity_negative * (prob_likelihood_negative_Y() * probability_of_negativity())
               :posterior_probabiity_negative = 0;
      return posterior_probabiity_negative;
    }

    private double cal_Prob_pos(bool found_x)
    {
        posterior_probabilty_positive = found_x == true?posterior_probabilty_positive = posterior_probabilty_positive * (prob_likelihood_positive_Y() * probability_of_positivity())
            : posterior_probabilty_positive = 0;
        return posterior_probabilty_positive ;
    }

    public string get_comment_status()
    {

        
        classification_status = pos_count==neg_count? "Neutral"
            : posterior_probabilty_positive < posterior_probabiity_negative ? "Negative"
            : posterior_probabilty_positive > posterior_probabiity_negative ? "Positive"
            : "Neutral";
        return classification_status;
    }
    


   


}
