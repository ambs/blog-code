import stanza
import re
import math

stanza.download("en")


def tokenize(filename):
    with open(filename, "r") as fh:
        content = fh.read()
    pipeline = stanza.Pipeline(lang='en', processors='tokenize', use_gpu=True)
    return pipeline(content)


def check_word(w):
    return w if re.fullmatch(r'\w+', w) and not re.match(r'\d', w) else "<OTHER>"


def preprocess(text):
    return [["<S>"] + [check_word(w.text.lower()) for w in s.words] + ["</S>"] for s in text.sentences]


def count_frequences(text):
    total_tokens = 0
    total_bigrams = 0
    tokens = {}
    bigrams = {}
    for sentence in text:
        total_tokens += len(sentence) - 2
        total_bigrams += len(sentence) - 1
        for pos in range(0, len(sentence) - 1):
            w1, w2 = sentence[pos], sentence[pos+1]
            if pos > 1:
                tokens[w1] = tokens.get(w1, 0) + 1
            bigrams[(w1, w2)] = bigrams.get((w1, w2), 0) + 1
    return tokens, total_tokens, bigrams, total_bigrams


def top_occurring(freqs, top_n=20):
    sorted_list = sorted(freqs.items(), key=lambda p: p[1], reverse=True)
    return sorted_list[:top_n]


def stop_words():
    return {"the", "be", "to", "of", "and", "a", "in", "that", "have", "I", "it", "for", "not", "on", "with",
            "he", "as", "you", "do", "at", "this", "but", "his", "by", "from", "they", "we", "say", "her", "she",
            "or", "an", "will", "my", "one", "all", "would", "there", "their", "what", "so", "up", "out", "if",
            "about", "who", "get", "which", "go", "me", "when", "make", "can", "like", "time", "no", "just", "him",
            "know", "take", "person", "into", "year", "your", "good", "some", "could", "them", "see", "other",
            "than", "then", "now", "look", "only", "come", "its", "over", "think", "also", "back", "after", "use",
            "two", "how", "our", "work", "first", "well", "way", "even", "new", "want", "because", "any", "these",
            "give", "day", "most", "us", "<S>", "</S>", "<OTHER>"}


def filtered_freqs(freqs):
    stop_word_set = stop_words()
    return {pair: freqs[pair] for pair in freqs.keys() if pair[0] not in stop_word_set and pair[1] not in stop_word_set}


def marginal_counts(freqs):
    left_set = {}
    right_set = {}
    for w1, w2 in freqs.keys():
        left_set[w1] = left_set.get(w1, 0) + freqs[(w1, w2)]
        right_set[w2] = right_set.get(w2, 0) + freqs[(w1, w2)]
    return left_set, right_set


def jaccard(u_freqs, bi_freqs, thereshold=5):
    left_set, right_set = marginal_counts(bi_freqs)
    return {
        (w1, w2): bi_freqs[(w1, w2)] / (left_set[w1] + right_set[w2] - bi_freqs[(w1, w2)]) for w1, w2 in bi_freqs.keys()
           if w1 in u_freqs and w2 in u_freqs and abs(u_freqs[w1] - u_freqs[w2]) > thereshold
    }


def sdc(u_freqs, bi_freqs, thereshold=5):
    left_set, right_set = marginal_counts(bi_freqs)
    return {
        (w1, w2): 2 * bi_freqs[(w1, w2)] / (left_set[w1] + right_set[w2]) for w1, w2 in bi_freqs.keys()
           if w1 in u_freqs and w2 in u_freqs and abs(u_freqs[w1] - u_freqs[w2]) > thereshold
    }


def pmi(u_freqs, u_total, bi_freqs, bi_total, thereshold=5):
    bi_prob = lambda x: bi_freqs[x] / bi_total
    u_prob = lambda x: u_freqs[x] / u_total
    return {
        (w1, w2): math.log2(bi_prob((w1, w2)) / (u_prob(w1) * u_prob(w2))) for w1, w2 in bi_freqs.keys()
           if w1 in u_freqs and w2 in u_freqs and abs(u_freqs[w1] - u_freqs[w2]) > thereshold
    }


tokens = tokenize("top_100k.txt")
tokens = preprocess(tokens)

token_frequencies, n_tokens, bi_frequencies, n_bigrams = count_frequences(tokens)

jaccards = jaccard(token_frequencies, bi_frequencies)
for pair, c in top_occurring(jaccards):
    print(f"{pair} => {c}")


sdcs = sdc(token_frequencies, bi_frequencies)
for pair, c in top_occurring(sdcs):
    print(f"{pair} => {c}")


pmis = pmi(token_frequencies, n_tokens, bi_frequencies, n_bigrams)
for pair, c in top_occurring(pmis):
    print(f"{pair} => {c}")



