using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using WordNetApp.Interface;

namespace WordNetApp.Models
{
    public class SynonymFilter : TokenFilter
    {
        private Queue<Token> synonymTokenQueue = new Queue<Token>();

        public ISynonymEngine SynonymEngine { get; private set; }

        public SynonymFilter(TokenStream input, ISynonymEngine synonymEngine): base(input)
        {
            if (synonymEngine == null)
                throw new ArgumentNullException(nameof(synonymEngine));

            SynonymEngine = synonymEngine;
        }

        public override Token Next()
        {
            // if our synonymTokens queue contains any tokens, return the next one.
            if (synonymTokenQueue.Count > 0)
            {
                return synonymTokenQueue.Dequeue();
            }

            //get the next token from the input stream
            var token = input.Next();

            //if the token is null, then it is the end of stream, so return null
            if (token == null)
                return null;

            //retrieve the synonyms
            var synonyms = SynonymEngine.GetSynonyms(token.TermText());

            //if we don't have any synonyms just return the token
            if (synonyms == null)
            {
                return token;
            }

            //if we do have synonyms, add them to the synonymQueue, 
            // and then return the original token
            foreach (var syn in synonyms)
            {
                //make sure we don't add the same word 
                if (token.TermText().Equals(syn)) continue;
                //create the synonymToken
                var synToken = new Token(syn, token.StartOffset(), token.EndOffset(), "<SYNONYM>");

                // set the position increment to zero
                // this tells lucene the synonym is 
                // in the exact same location as the originating word
                synToken.SetPositionIncrement(0);

                //add the synToken to the synonyms queue
                synonymTokenQueue.Enqueue(synToken);
            }

            //after adding the syn to the queue, return the original token
            return token;
        }
    }
}
