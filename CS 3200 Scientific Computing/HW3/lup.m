function [L,U,P] = lup(A)
%lup takes in a matrix A and outputs
%L, U, P matrices for LU Decomposition
    
    [m,n] = size(A); %get size of A
    L = eye(m,n); %initialize L to identity  
    P = eye(m,n); %initialize P to identity
    U = A; %initialize U to A
    for i = 1:n-1
       for j = i+1:n
           if(U(i,i)==0) 
           [L,U,P] = pivot(i,j-1, P, U, L); 
             break;
           end
            L(j,i)=U(j,i)/U(i,i); %add to L matrix
            U(j,i:n) = U(j,i:n)-L(j,i)*U(i,i:n); %change U
       end
    end
end

function[L,U,P] = pivot(i,j,P, U, L)
        temp = 0;
        index = 1;
        n= size(P);
    for c = i:i
       for r = j:n
           if(U(r,c) > temp) %check for greatest number in column
              temp = U(r,c);
              
           end
           index = index + 1; 
       end
    end
  %swap rows in P, U, and L  
  row =  P(j,:);
  P(j,:) = P(index,:);
  P(index,:) = row;
  
  row =  U(j,:);
  U(j,:) = U(index,:);
  U(index,:) = row;
  
  row =  L(j,:);
  L(j,:) = L(index,:);
  L(index,:) = row;
    
end