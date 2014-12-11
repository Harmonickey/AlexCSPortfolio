;;; This is the IDE's built-in-editor, where you create and edit
;;; lisp source code.  You could use some other editor instead,
;;; though the IDE's menu-bar commands would not be applicable there.
;;; 
;;; This editor has a tab for each file that it's editing.  You can
;;; create a new editor buffer at any time with the File | New command.
;;; Other commands such as Search | Find Definitions will create
;;; editor buffers automatically for existing code.
;;; 
;;; You can use the File | Compile and Load command to compile and
;;; load an entire file, or compile an individual definition by
;;; placing the text cursor inside it and using Tools | Incremental
;;; Compile.  You can similarly evaluate test expressions in the
;;; editor by using Tools | Incremental Evaluation; the returned
;;; values and any printed output will appear in a lisp listener
;;; in the Debug Window.
;;; 
;;; For a brief introduction to other IDE tools, try the
;;; Help | Interactive IDE Intro command.  And be sure to explore
;;; the other facilities on the Help menu.

(in-package :boggle)

(defpackage trie
  (:use :common-lisp lisp-unit)
  (:export :trie :make-trie :add-word :add-word-with-offset :subtrie :trie-word :trie-count :trie-count-helper :read-words))

(defpackage boggle
  (:use :common-lisp lisp-unit)
  (:export :load-words :solve-boggle :set-place :get-letter-from-string 
           :get-letter-from-vector :solve :get-position-pairs :navigate :valid-position-p))

(defvar *word-base* nil)

;; Trie Functions

(defun read-words (words trie)
  (with-open-file (stream words)
    (do ((l (read-line stream nil 'eof) (read-line stream nil 'eof)))
        ((eq l 'eof) trie)
      (add-word l trie))))

(defun new-trie (&rest words)
  (add-words words (make-trie)))

(defun add-words (words trie)
  (dolist (word words trie)
    (add-word word trie)))  

(defstruct trie
  (word nil) (count 0) (branches nil))

(defun traverse-trie (trie word action)
  (let ((letters (coerce word 'list)))
    (if (null (car letters))
       trie
     (let* ((letter (coerce (car letters) 'character))
            (matched-trie (branch-matches trie letter)))
       (cond 
        ((and (eql action 'add)
              (null matched-trie))
         (incf (trie-count trie))
         (attach-branch trie letter)
         (traverse-trie trie letters action)) 
        (matched-trie
         (traverse-trie (cdr matched-trie) (cdr letters) action))
        (t nil))))))

(defun branch-matches (trie letter)
  (assoc-if #'(lambda (branch)
                (char-equal letter branch))
            (trie-branches trie)))

(defun add-word (word trie)
  (let ((word-branch (traverse-trie trie word 'add)))
    (setf (trie-word word-branch) word)))

(defun attach-branch (trie letter)
  (setf (trie-branches trie) ;;attach new branch
    (acons letter (make-trie) (trie-branches trie))))

(defun get-word (branch default)
  (if (null branch)
      default
    (trie-word branch)))

;; Boggle Functions

(defstruct boggle-board
  (size) (data))

(defun load-words (pathname)
  (setq *word-base* (read-words pathname (make-trie))))

(defun solve-boggle (board)
  (let ((dim (round (sqrt (length board))))
        (b (make-boggle-board)))
    (setf (boggle-board-size b) dim)
    (setf (boggle-board-data b) (make-array (* dim dim)))
    (loop for i below dim do
          (loop for j below dim do
                (set-place b i j (get-letter-from-string board i j dim))))
    (sort-results 
             (mapcan #'(lambda (pair)
                         (solve b (car pair) (cdr pair) nil nil))
               (get-position-pairs dim)))))

(defun solve (board i j running-word visited)
  (let ((dim (boggle-board-size board)))
    (if (not (valid-position-p i j dim visited))
        nil
      (let* ((test-word (append (coerce running-word 'list) 
                                (list (get-letter-from-vector board i j))))
             (matched-branch (traverse-trie *word-base* test-word 'read)))
        (if (or (null matched-branch) 
                (null (cddr test-word))
                (null (trie-word matched-branch)))
            (navigate board i j dim visited test-word)            
        (cons (trie-word matched-branch) 
              (navigate board i j dim visited test-word)))))))

(defun navigate (board i j dim visited word)
  (let ((past-positions (cons (+ (* i dim) j) visited)))
    (append
     (solve board (1+ i) j word past-positions)
     (solve board i (1+ j) word past-positions)
     (solve board (1+ i) (1+ j) word past-positions)
     (solve board (1- i) j word past-positions) 
     (solve board i (1- j) word past-positions) 
     (solve board (1- i) (1- j) word past-positions) 
     (solve board (1- i) (1+ j) word past-positions)
     (solve board (1+ i) (1- j) word past-positions))))
                
(defun valid-position-p (i j dim visited)
  (not (or (= i dim)
           (= j dim)
           (< i 0)
           (< j 0)
           (member (+ (* i dim) j) visited))))

(defun sort-results (words)
  (sort (remove-duplicates (remove nil words)) #'string-greaterp))

(defun get-position-pairs (dim)
  (loop for i below dim append
    (loop for j below dim collect (cons i j))))
         
(defun get-letter-from-string (board-list i j dim)
  (char board-list (+ (* i dim) j)))

(defun get-letter-from-vector (board i j)
  (aref (boggle-board-data board) (+ (* i (boggle-board-size board)) j)))
    
(defun set-place (b i j value)
  (setf (aref (boggle-board-data b) (+ (* i (boggle-board-size b)) j)) value))
